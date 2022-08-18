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
using OnlineCash.Extensions;

namespace OnlineCash.Services
{
    public class ArrivalService
    {
        private readonly shopContext db;
        private readonly IConfiguration _configuration;
        private readonly RevaluationService _revaluationService;
        private readonly ILogger<ArrivalService> _logger;
        private readonly RabbitService _rabbitService;
        public ArrivalService(shopContext db,
            IConfiguration configuration,
            RevaluationService revaluationService,
            ILogger<ArrivalService> logger,
            RabbitService rabbitService)
        {
            this.db = db;
            _configuration = configuration;
            _revaluationService = revaluationService;
            _logger = logger;
            _rabbitService = rabbitService;
        }

        public async Task SaveSynchAsync(int shopId, ArrivalSynchModel model, Guid? uuiSynch)
        {
            try
            {
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
                        PriceSell = mGood.PriceSell,
                        Count = mGood.Count,
                        Nds = mGood.Nds,
                        ExpiresDate = mGood.ExpiresDate
                    });
                }
                db.ArrivalGoods.AddRange(arrivalGoods);
                arrival.SumNds = arrival.ArrivalGoods.Sum(a => a.SumNds);
                arrival.SumArrival = arrival.ArrivalGoods.Sum(a => a.Sum);
                arrival.SumSell = arrival.ArrivalGoods.Sum(a => a.SumSell);

                db.SaveChanges();
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
                    await CreateRevaluation(arrivalGoods);
                    SendBalanceAndNotify(arrival.Id, arrival.DateArrival, arrival.Supplier?.Name, arrivalGoods);
                }
                db.DocumentHistories.Add(new DocumentHistory { TypeDoc = TypeDocs.Arrival, DocId = arrival.Id });
                await db.SaveChangesAsync();
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
                Status = model.isSuccess ? DocumentStatus.Confirm : DocumentStatus.New,
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
            //Подсчет итогов
            arrival.SumArrival = model.ArrivalGoods.Sum(a => a.Sum);
            arrival.SumNds = model.ArrivalGoods.Sum(a => a.SumNds);
            arrival.SumSell = model.ArrivalGoods.Sum(a => a.SumSell);
            await db.SaveChangesAsync();
            if (arrival.Status == DocumentStatus.Confirm)
            {
                db.DocumentHistories.Add(new DocumentHistory { TypeDoc = TypeDocs.Arrival, DocId = arrival.Id });
                await db.SaveChangesAsync();
                await CreateRevaluation(arrivalGoods);
                SendBalanceAndNotify(arrival.Id, arrival.DateArrival, arrival.Supplier?.Name, arrivalGoods);
            }
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
                    
            foreach (var modelGood in model.ArrivalGoods)
            {
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
                    var arrivalGood = arrival.ArrivalGoods.Where(a => a.Id == modelGood.Id).FirstOrDefault();
                    arrivalGood.Price = modelGood.Price;
                    arrivalGood.PriceSell = modelGood.PriceSell;
                    arrivalGood.Count = modelGood.Count;
                    arrivalGood.Nds = modelGood.Nds;
                    arrivalGood.ExpiresDate = modelGood.ExpiresDate;
                };
            }
            arrival.SumArrival = model.ArrivalGoods.Sum(a => a.Sum);
            arrival.SumNds = model.ArrivalGoods.Sum(a => a.SumNds);
            arrival.SumSell = model.ArrivalGoods.Sum(a => a.SumSell);
            await db.SaveChangesAsync();
            arrival.ArrivalGoods = model.ArrivalGoods;
            if (documentStatusOld != DocumentStatus.Confirm & arrival.Status == DocumentStatus.Confirm)
            {
                db.DocumentHistories.Add(new DocumentHistory { TypeDoc = TypeDocs.Arrival, DocId = arrival.Id });
                await db.SaveChangesAsync();
                await CreateRevaluation(arrival.ArrivalGoods);
                SendBalanceAndNotify(
                    arrival.Id, 
                    arrival.DateArrival, 
                    arrival.Supplier?.Name, 
                    arrival.ArrivalGoods);
            }
        }

        /// <summary>
        /// Создает документы переоценки, уведомления о изменение цены, изменение количества товара
        /// </summary>
        /// <param name="arrival"></param>
        /// <returns></returns>
        private void SendBalanceAndNotify(int arrivalId, 
            DateTime dateArrival, 
            string? supplier=null, 
            IEnumerable<ArrivalGood> arrivalPlus=null,
            IEnumerable<ArrivalGood> arrivalMinus=null)
        {
            List<GoodBalanceSynchModel> balancesPlus = new();
            List<GoodBalanceSynchModel> balancesMinnus = new();
            if (arrivalMinus != null)
                foreach(var balance in arrivalMinus)
                    balancesMinnus.Add(new GoodBalanceSynchModel
                    {
                        DocumentId = arrivalId,
                        DocumentDate = dateArrival,
                        TypeDoc = TypeDocs.Arrival,
                        GoodId = balance.GoodId,
                        Count = -1 * balance.Count
                    });
            if (arrivalPlus != null)
                foreach (var balance in arrivalPlus)
                    balancesPlus.Add(new GoodBalanceSynchModel
                    {
                        DocumentId = arrivalId,
                        DocumentDate = dateArrival,
                        TypeDoc = TypeDocs.Arrival,
                        GoodId = balance.GoodId,
                        Count = balance.Count
                    });
            _rabbitService.Send<List<GoodBalanceSynchModel>>(RabbitService.QueueNames.GoodBalance, balancesMinnus);
            _rabbitService.Send<List<GoodBalanceSynchModel>>(RabbitService.QueueNames.GoodBalance, balancesPlus);

            _rabbitService.Send<TelegramNotifyModel>(RabbitService.QueueNames.Notify,
                new TelegramNotifyModel
                {
                    Message = $"Приходная накладная от {supplier} на сумму {arrivalPlus.Sum(a=>a.Sum)}",
                    Url = "Arrivals/Edit?ArrivalId=" + arrivalId
                });
        }

        public async Task CreateRevaluation(IEnumerable<ArrivalGood> arrivalGoods)
        {
            List<RevaluationAddModel> priceChanged = new List<RevaluationAddModel>();

            foreach (var aGood in arrivalGoods)
            {
                var good = await db.Goods.Include(g => g.GoodPrices)
                    .Where(g => g.Id == aGood.GoodId).FirstOrDefaultAsync();
                if (good.Price != aGood.PriceSell)
                {
                    priceChanged.Add(new RevaluationAddModel { 
                        Good = good, 
                        PriceOld = good.Price, 
                        PriceNew = aGood.PriceSell 
                    });
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
                _rabbitService.Send<TelegramNotifyModel>(RabbitService.QueueNames.Notify,
                    new TelegramNotifyModel
                    {
                        Message = message
                    });
                await _revaluationService.Create(priceChanged);
            }

        }
    }
}
