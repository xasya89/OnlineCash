using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OnlineCash.Services
{
    public class ArrivalService
    {
        shopContext db;
        IConfiguration _configuration;
        GoodCountBalanceService _countBalanceService;
        NotificationOfEventInSystemService _notification;
        RevaluationService _revaluationService;
        ILogger<ArrivalService> _logger;
        public ArrivalService(shopContext db,
            IConfiguration configuration,
            IGoodBalanceService goodBalanceService,
            GoodCountBalanceService countBalanceService,
            RevaluationService revaluationService,
            NotificationOfEventInSystemService notification,
            ILogger<ArrivalService> logger)
        {
            this.db = db;
            _configuration = configuration;
            _countBalanceService = countBalanceService;
            _notification = notification;
            _revaluationService = revaluationService;
            _logger = logger;
        }

        public async Task SaveSynchAsync(int shopId, ArrivalSynchModel model, Guid? uuiSynch)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _configuration.GetSection("RabbitServer").Value,
                    UserName = _configuration.GetSection("RabbitUser").Value,
                    Password = _configuration.GetSection("RabbitPassword").Value
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare("shop_test", "direct", true);
                channel.QueueDeclare(queue: "shop_test_goodbalance",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.QueueBind("shop_test_goodbalance", "shop_test", "shop_test_goodbalance");

                if (db.Suppliers.Where(s => s.Id == model.SupplierId).FirstOrDefault() == null)
                    throw new Exception($"Поставщик supplierId - {model.SupplierId} не найден");
                var goods = db.Goods.Include(g => g.GoodPrices.Where(gp => gp.ShopId == 1)).AsNoTracking().ToList();
                foreach (var mGood in model.ArrivalGoods)
                    if (goods.Where(g => g.Uuid == mGood.GoodUuid).FirstOrDefault() == null)
                        throw new Exception($"Товар uuid {mGood.GoodUuid} не найден");

                bool autoSuccess = _configuration.GetSection("AutoSuccessFromCash").Value == "1";
                var arrival = new Arrival
                {
                    Num = model.Num,
                    DateArrival = model.DateArrival,
                    ShopId = 1,
                    SupplierId = model.SupplierId,
                    SumSell = 0,
                    isSuccess = autoSuccess
                };
                db.Arrivals.Add(arrival);
                List<ArrivalGood> arrivalGoods = new List<ArrivalGood>();
                foreach (var mGood in model.ArrivalGoods)
                {
                    var good = goods.Where(g => g.Uuid == mGood.GoodUuid).FirstOrDefault();
                    arrivalGoods.Add(new ArrivalGood
                    {
                        Arrival = arrival,
                        GoodId = good.Id,
                        Price = mGood.Price,
                        PriceSell = good.GoodPrices.FirstOrDefault().Price,
                        Count = mGood.Count,
                        Nds = mGood.Nds,
                        ExpiresDate = mGood.ExpiresDate
                    });
                }
                db.ArrivalGoods.AddRange(arrivalGoods);
                arrival.SumNds = arrival.ArrivalGoods.Sum(a => a.SumNds);
                arrival.SumArrival = arrival.ArrivalGoods.Sum(a => a.Sum);
                arrival.SumSell = arrival.ArrivalGoods.Sum(a => a.SumSell);

                var docSynch = await db.DocSynches.Where(d => d.Uuid == uuiSynch).FirstOrDefaultAsync();
                if (docSynch != null)
                {
                    docSynch.DocId = arrival.Id;
                    docSynch.TypeDoc = TypeDocs.Arrival;
                    docSynch.isSuccess = true;
                }
                db.SaveChanges();

                if (autoSuccess)
                {
                    List<GoodBalanceSynchModel> balances = new();
                    foreach (var arrivalGood in arrivalGoods)
                        balances.Add(new GoodBalanceSynchModel
                        {
                            DocumentId = arrival.Id,
                            DocumentDate = arrival.DateArrival,
                            TypeDoc = TypeDocs.Arrival,
                            GoodId = arrivalGood.GoodId,
                            Count = arrivalGood.Count
                        });
                    var bodyBalances = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(balances));
                    channel.BasicPublish(exchange: "shop_test",
                     routingKey: "shop_test_goodbalance",
                     basicProperties: null,
                     body: bodyBalances);
                }

                await _notification.Send($"Приходная накладная {arrival.Num} на сумму {arrival.SumArrival}", "Arrivals/Edit?ArrivalId=" + arrival.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Arrival background service error - " + ex.Message);
            }
        }

        public async Task Create(Arrival model)
        {
            Arrival arrival = null;
            decimal priceAll = model.ArrivalGoods.Sum(g => g.Price);
            decimal countAll = model.ArrivalGoods.Sum(g => g.Count);
            arrival = new Arrival
            {
                Status=model.isSuccess ? DocumentStatus.Confirm : DocumentStatus.New,
                Num = model.Num,
                DateArrival = model.DateArrival,
                ShopId = model.ShopId,
                SupplierId = model.SupplierId,
                isSuccess = model.isSuccess
            };
            db.Arrivals.Add(arrival);

            var arrivalGoods = new List<ArrivalGood>();
            foreach (var modelGood in model.ArrivalGoods)
            {
                arrivalGoods.Add(new ArrivalGood
                {
                    Arrival = arrival,
                    GoodId = modelGood.GoodId,
                    Price = modelGood.Price,
                    PriceSell = modelGood.PriceSell,
                    Count = modelGood.Count,
                    Nds = modelGood.Nds,
                    ExpiresDate = modelGood.ExpiresDate
                });
                arrival.SumArrival += modelGood.Price * modelGood.Count;
                arrival.SumSell += modelGood.PriceSell * modelGood.Count;
                arrival.SumNds += modelGood.SumNds;
            };
            db.ArrivalGoods.AddRange(arrivalGoods);
            if (model.isSuccess)
            //Подсчет итогов
            arrival.SumArrival = model.ArrivalGoods.Sum(a => a.Sum);
            arrival.SumNds = model.ArrivalGoods.Sum(a => a.SumNds);
            arrival.SumSell = model.ArrivalGoods.Sum(a => a.SumSell);
            await db.SaveChangesAsync();
            arrival.ArrivalGoods = arrivalGoods;
            await ActionSuccess(arrival);
        }

        public async Task Edit(Arrival model)
        {
            var documentStatusOld = model.Status;
            var arrival = await db.Arrivals.Include(a=>a.ArrivalGoods).ThenInclude(a=>a.Good).Where(a => a.Id == model.Id).FirstOrDefaultAsync();
            if (arrival == null)
                throw new Exception("Документ прихода не найден, id "+arrival.Id);
            arrival.Num = model.Num;
            arrival.DateArrival = model.DateArrival;
            arrival.ShopId = model.ShopId;
            arrival.SupplierId = model.SupplierId;
            arrival.isSuccess = model.isSuccess;
            arrival.Status = model.isSuccess ? DocumentStatus.Confirm : model.Status;
            //Найдем удаленные товары
            foreach (var arrivalgood in arrival.ArrivalGoods)
                if (model.ArrivalGoods.Where(m => m.Id == arrivalgood.Id).FirstOrDefault() == null)
                    db.Remove(arrivalgood);
            //
            foreach (var modelGood in model.ArrivalGoods)
                if (modelGood.Id == -1)
                {
                    var good = await db.Goods.Where(g => g.Id == modelGood.GoodId).FirstOrDefaultAsync();
                    if (good == null)
                        throw new Exception($"Товар с id {modelGood.Id} не найден");
                    var arrivalGood = new ArrivalGood
                    {
                        Arrival = arrival,
                        Good = good,
                        Price = modelGood.Price,
                        PriceSell = modelGood.PriceSell,
                        Count = modelGood.Count,
                        Nds = modelGood.Nds,
                        ExpiresDate = modelGood.ExpiresDate
                    };
                    db.ArrivalGoods.Add(arrivalGood);
                }
                else
                {
                    var arrivalGood = arrival.ArrivalGoods.Where(a=>a.Id==modelGood.Id).FirstOrDefault();
                    arrivalGood.Price = modelGood.Price;
                    arrivalGood.PriceSell = modelGood.PriceSell;
                    arrivalGood.Count = modelGood.Count;
                    arrivalGood.Nds = modelGood.Nds;
                    arrivalGood.ExpiresDate = modelGood.ExpiresDate;
                };
            arrival.SumArrival = model.ArrivalGoods.Sum(a => a.Sum);
            arrival.SumNds = model.ArrivalGoods.Sum(a => a.SumNds);
            arrival.SumSell = model.ArrivalGoods.Sum(a => a.SumSell);
            await db.SaveChangesAsync();
            arrival.ArrivalGoods = model.ArrivalGoods;
            if (documentStatusOld != DocumentStatus.Confirm)
                await ActionSuccess(arrival);
        }

        /// <summary>
        /// Создает документы переоценки, уведомления о изменение цены, изменение количества товара
        /// </summary>
        /// <param name="arrival"></param>
        /// <returns></returns>
        private async Task ActionSuccess(Arrival arrival)
        {
            if (arrival.Status == DocumentStatus.Confirm)
            {
                var supplier = await db.Suppliers.Where(s => s.Id == arrival.SupplierId).FirstOrDefaultAsync();
                await _notification.Send($"Приходная накладная {arrival.Num} поставщик {supplier.Name} на сумму {arrival.SumArrival}", "Arrivals/Edit?ArrivalId=" + arrival.Id);
                List<RevaluationAddModel> priceChanged = new List<RevaluationAddModel>();

                foreach (var aGood in arrival.ArrivalGoods)
                {
                    var good = await db.Goods.Include(g => g.GoodPrices).Where(g => g.Id == aGood.GoodId).FirstOrDefaultAsync();
                    if (good.Price != aGood.PriceSell)
                    {
                        //priceChanged.Add(new { GoodName = good.Name, PriceOld = good.Price, PriceNew = aGood.PriceSell });
                        priceChanged.Add(new RevaluationAddModel { Good = good, PriceOld = good.Price, PriceNew = aGood.PriceSell });
                        good.Price = aGood.PriceSell;
                        foreach (var price in good.GoodPrices)
                            price.Price = aGood.PriceSell;
                        await db.SaveChangesAsync();
                    }
                }
                if (priceChanged.Count > 0)
                {
                    string message = "Изменилась цена:";
                    foreach (var price in priceChanged)
                        message += $"\n{price.Good.Name} с {price.PriceOld} по {price.PriceNew}";
                    await _notification.Send(message);
                    await _revaluationService.Create(priceChanged);
                }
                await _countBalanceService.Add<ArrivalGood>(arrival.Id, DateTime.Now, arrival.ArrivalGoods);
            };
        }
    }
}
