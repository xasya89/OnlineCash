using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;
using OnlineCashRmk.DataModels;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Flurl.Http;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace OnlineCashRmk.Services
{
    class SynchBackgroundService : BackgroundService
    {
        private readonly DataContext db;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        string serverurl;
        int shopId;
        int cronSynch;
        private readonly IServiceScopeFactory _scopeFactory;

        public SynchBackgroundService(IServiceScopeFactory scopeFactory, 
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            serverurl = _configuration.GetSection("serverName").Value;
            shopId = Convert.ToInt32(_configuration.GetSection("idShop").Value);
            cronSynch = Convert.ToInt32(_configuration.GetSection("Crons").GetSection("Synch").Value);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<IDbContextFactory<DataContext>>().CreateDbContext();
            /*
            var factory = new ConnectionFactory()
            {
                HostName = _configuration.GetSection("RabbitServer").Value,
                UserName = _configuration.GetSection("RabbitUser").Value,
                Password = _configuration.GetSection("RabbitPassword").Value
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare("shop_test", "direct", true);
            channel.QueueDeclare(queue: "shop_test_arrivals",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind("shop_test_arrivals", "shop_test", "shop_test_arrivals");
            */
            /*
            Task.Run(async () =>
            {
                try
                {
                    await GetBuyersAsync();
                }
                catch (FlurlHttpException) { }
                catch (HttpRequestException) { }
                catch (Exception) { };
            });
            */
            while (!stoppingToken.IsCancellationRequested)
                {
                    IEnumerable<DocSynch> docs = await db.DocSynches.Where(d => d.SynchStatus == false).OrderBy(d => d.Create).ToListAsync();
                    try
                    {
                        foreach (var doc in docs)
                        {
                            if (doc.Uuid.ToString() == "00000000-0000-0000-0000-000000000000")
                                doc.Uuid = Guid.NewGuid();
                            switch (doc.TypeDoc)
                            {
                                case TypeDocs.OpenShift:
                                    var shift = await db.Shifts.Where(s => s.Id == doc.DocId).FirstOrDefaultAsync();
                                    using (var client = _httpClientFactory.CreateClient())
                                    {
                                        client.DefaultRequestHeaders.Add("doc-uuid", doc.Uuid.ToString());
                                        var resp = await client.PostAsync($"{serverurl}/api/onlinecash/Cashbox/OpenShift/{shopId}/{shift.Uuid}/{shift.Start.ToString()}", null);
                                        if (resp.IsSuccessStatusCode)
                                        {
                                            doc.SynchStatus = true;
                                            doc.Synch = DateTime.Now;
                                            await db.SaveChangesAsync();
                                        }
                                        else
                                            throw new Exception("Ошибка отправки на сервер");
                                    }
                                    break;
                                case TypeDocs.Buy:
                                    await SendCheckSell(db, doc.DocId);
                                    doc.SynchStatus = true;
                                    doc.Synch = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    break;
                                case TypeDocs.CloseShift:
                                    using (var client = _httpClientFactory.CreateClient())
                                    {
                                        client.DefaultRequestHeaders.Add("doc-uuid", doc.Uuid.ToString());
                                        var shiftclose = await db.Shifts.Where(s => s.Id == doc.DocId).FirstOrDefaultAsync();
                                        var resp = await client.PostAsync($"{serverurl}/api/onlinecash/Cashbox/CloseShift/{shiftclose.Uuid}/{shiftclose.Stop.ToString()}", null);
                                        if (resp.IsSuccessStatusCode)
                                        {
                                            doc.SynchStatus = true;
                                            doc.Synch = DateTime.Now;
                                            await db.SaveChangesAsync();
                                        }
                                        else
                                            throw new Exception("Ошибка отправки на сервер");
                                    }
                                    break;
                                case TypeDocs.WriteOf:
                                    var writeof = await db.Writeofs.Include(w => w.WriteofGoods).ThenInclude(wg => wg.Good).Where(w => w.Id == doc.DocId).FirstOrDefaultAsync();
                                    List<WriteofGoodSynchDataModel> goods = new List<WriteofGoodSynchDataModel>();
                                    foreach (var wg in writeof.WriteofGoods)
                                        goods.Add(new WriteofGoodSynchDataModel { Uuid = wg.Good.Uuid, Count = wg.Count, Price = wg.Price });
                                    var writeofsynch = new WriteofSynchDataModel { DateCreate = writeof.DateCreate, Note = writeof.Note, Goods = goods };
                                    using (var client = new HttpClient())
                                    {
                                        client.DefaultRequestHeaders.Add("doc-uuid", doc.Uuid.ToString());
                                        var resp = await client.PostAsJsonAsync($"{serverurl}/api/onlinecash/WriteofSynch/{shopId}", writeofsynch);
                                        if (resp.IsSuccessStatusCode)
                                        {
                                            doc.SynchStatus = true;
                                            doc.Synch = DateTime.Now;
                                            await db.SaveChangesAsync();
                                        }
                                    }
                                    break;
                                case TypeDocs.Arrival:
                                    //TODO: Synch Arrival вернутьс Rabbit
                                    //var model = await SendArrival(channel, db, doc);
                                    doc.SynchStatus = true;
                                    doc.Synch = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    break;
                                case TypeDocs.StockTaking:
                                    await SendStocktaking(doc);
                                    doc.SynchStatus = true;
                                    doc.Synch = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    break;
                                case TypeDocs.StartStocktacking:
                                    await SendStartStocktacking(doc);
                                    doc.SynchStatus = true;
                                    doc.Synch = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    break;
                                case TypeDocs.StopStocktacking:
                                    await SendStopStocktacking(doc);
                                    doc.SynchStatus = true;
                                    doc.Synch = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    break;
                                case TypeDocs.CashMoney:
                                    await SendCashMoney(doc);
                                    doc.SynchStatus = true;
                                    doc.Synch = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    break;
                                case TypeDocs.NewGoodFromCash:
                                    await SendNewGood(doc);
                                    doc.SynchStatus = true;
                                    doc.Synch = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    break;
                                case TypeDocs.Revaluation:
                                    await SendRevaluation(doc);
                                    doc.SynchStatus = true;
                                    doc.Synch = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    break;
                            }
                        }
                    }
                    catch (FlurlHttpException) { }
                    catch (HttpRequestException) { }
                    catch (Exception ex) 
                    { 
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace); 
                    }
                    await Task.Delay(TimeSpan.FromMinutes(cronSynch));
                }

        }

        private async Task<ArrivalSynchDataModel> SendArrival(IModel channel, DataContext db, DocSynch docSynch)
        {
            var arrival = await db.Arrivals.Include(a => a.ArrivalGoods).ThenInclude(a => a.Good).Where(a => a.Id == docSynch.DocId).FirstOrDefaultAsync();
            if (arrival == null)
                return null;
            var model = new ArrivalSynchDataModel { Num = arrival.Num, DateArrival = arrival.DateArrival, SupplierId = arrival.SupplierId, Uuid = arrival.Uuid };
            foreach (var aGood in arrival.ArrivalGoods)
                model.ArrivalGoods.Add(new ArrivalGoodSynchDataModel
                {
                    GoodUuid = aGood.Good.Uuid,
                    Price = aGood.Price,
                    Count = aGood.Count,
                    Nds=aGood.Nds,
                    ExpiresDate=aGood.ExpiresDate
                });
            /*
            await $"{serverurl}/api/onlinecash/ArrivalSynch/{shopId}"
                .WithHeaders(new { Doc_uuid = docSynch.Uuid.ToString() })
                .PostJsonAsync(model);
            */
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model));
            channel.BasicPublish(exchange: "shop_test",
         routingKey: "shop_test_arrivals",
         basicProperties: null,
         body: body);
            return model;
        }


        public async Task GetBuyersAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var buyers =  await client.GetFromJsonAsync<List<Buyer>>($"{serverurl}/api/onlinecash/Buyers");
            var buyersdb = await db.Buyers.ToListAsync();
            foreach (var buyer in buyers)
            {
                var buyerDb = buyersdb.Where(b => b.Uuid == buyer.Uuid).FirstOrDefault();
                if (buyerDb == null)
                {
                    buyerDb = buyer;
                    buyerDb.Id = 0;
                    db.Buyers.Add(buyerDb);
                }
                buyerDb.DiscountSum = buyer.DiscountSum;
                buyerDb.SpecialPercent = buyer.SpecialPercent;
                buyerDb.TemporyPercent = buyer.TemporyPercent;
            }
            await db.SaveChangesAsync();
        }

        public async Task GetDiscountSettings()
        {
            var discounts = await $"{serverurl}/api/onlinecash/Discounts".GetJsonAsync<DiscountParamContainerModel>();
            var discountsDb = await db.DiscountSettings.FirstOrDefaultAsync();
            if(discountsDb==null)
            {
                db.DiscountSettings.Add(new DiscountSetting { DiscountModel = discounts });
                await db.SaveChangesAsync();
            }
            else
            {
                discountsDb.DiscountModel = discounts;
            }
        }

        public async Task SendCheckSell(DataContext db, int docId)
        {
            var sell = db.CheckSells
                .Include(s => s.CheckGoods).ThenInclude(g=>g.Good)
                .Include(s => s.CheckPayments)
                .Include(s=>s.Buyer)
                .Where(s => s.Id == docId).FirstOrDefault();
            var shiftbuy = db.Shifts.Where(s => s.Id == sell.ShiftId).FirstOrDefault();
            CashBoxCheckSellBuyer cashBuyer = null;
            if (sell.Buyer != null)
                cashBuyer = new CashBoxCheckSellBuyer { Uuid = sell.Buyer.Uuid, Phone = sell.Buyer.Phone };
            decimal sumCash = sell.CheckPayments.Where(p => p.TypePayment == TypePayment.Cash).Sum(p => p.Sum);
            decimal sumElectron = sell.CheckPayments.Where(p => p.TypePayment == TypePayment.Electron).Sum(p => p.Sum);
            var sellPost = new CashBoxCheckSellModel
            {
                IsReturn=sell.IsReturn,
                Create = sell.DateCreate,
                Buyer = cashBuyer,
                SumCash = sell.CheckPayments.Where(p => p.TypePayment == TypePayment.Cash).Sum(p => p.Sum),
                SumElectron = sell.CheckPayments.Where(p => p.TypePayment == TypePayment.Electron).Sum(p => p.Sum),
                SumDiscount = sell.SumDiscont
            };
            foreach (var g in sell.CheckGoods)
                sellPost.Goods.Add(new CashBoxCheckSellGood
                {
                    Uuid = g.Good.Uuid,
                    Count = (decimal)g.Count,
                    Discount = 0,
                    Price = g.Cost
                });
            await $"{serverurl}/api/onlinecash/CashBox/sell/{shiftbuy.Uuid}".PostJsonAsync(sellPost);
        }

        public async Task SendStocktaking(DocSynch docSynch)
        {
            var stocktaking = await db.Stocktakings.Include(s => s.StocktakingGroups).ThenInclude(s => s.StocktakingGoods).ThenInclude(g => g.Good)
                .Where(s => s.Id == docSynch.DocId).FirstOrDefaultAsync();
            var stocktakingSend = new StocktakingSendDataModel { Create = stocktaking.Create };
            foreach (var group in stocktaking.StocktakingGroups)
            {
                var groupSend = new StocktakingGroupSendDataModel { Name = group.Name };
                foreach (var good in group.StocktakingGoods)
                {
                    double countfact = 0;
                    if (good.CountFact != null)
                        countfact = (double)good.CountFact;
                    groupSend.Goods.Add(new StocktakingGoodSendDataModel { Uuid = good.Uuid, CountFact = countfact });
                };
                stocktakingSend.Groups.Add(groupSend);
            }
            try
            {
                var result = await $"{serverurl}/api/onlinecash/Stocktaking/{shopId}".WithHeaders(new { Doc_uuid=docSynch.Uuid.ToString() }).PostJsonAsync(stocktakingSend);
            }
            catch (FlurlHttpException ex)
            {
            }
        }

        public async Task SendStartStocktacking(DocSynch docSynch)
        {
            var stocktaking = await db.Stocktakings.Where(s => s.Id == docSynch.DocId).FirstOrDefaultAsync();
            StocktakingSendDataModel model = new StocktakingSendDataModel { 
                Create = stocktaking.Create, Uuid=stocktaking.Uuid, CashMoney = stocktaking.CashMoney 
            };
            await $"{serverurl}/api/onlinecash/Stocktaking/start/{shopId}"
                .WithHeaders(new { Doc_uuid = docSynch.Uuid.ToString() })
                .PostJsonAsync(model);
        }

        public async Task SendStopStocktacking(DocSynch docSynch)
        {
            var stocktaking = await db.Stocktakings.Include(s => s.StocktakingGroups).ThenInclude(s => s.StocktakingGoods).ThenInclude(g => g.Good)
                .Where(s => s.Id == docSynch.DocId).FirstOrDefaultAsync();
            List<StocktakingGroupSendDataModel> groups = new List<StocktakingGroupSendDataModel>();
            foreach (var group in stocktaking.StocktakingGroups)
            {
                var groupSend = new StocktakingGroupSendDataModel { Name = group.Name };
                foreach (var good in group.StocktakingGoods)
                {
                    double countfact = 0;
                    if (good.CountFact != null)
                        countfact = (double)good.CountFact;
                    groupSend.Goods.Add(new StocktakingGoodSendDataModel { Uuid = good.Uuid, CountFact = countfact });
                };
                groups.Add(groupSend);
            }
            await $"{serverurl}/api/onlinecash/Stocktaking/stop/{stocktaking.Uuid}"
                .WithHeaders(new { Doc_uuid = docSynch.Uuid.ToString() })
                .PostJsonAsync(groups);
        }

        public async Task SendCashMoney(DocSynch docSynch)
        {
            var cashMoney = db.CashMoneys.Where(c => c.Id == docSynch.DocId).FirstOrDefault();
            if (cashMoney == null)
                throw new Exception("Не найден документ cashMoney");
            await $"{serverurl}/api/onlinecash/CashMoneys/{shopId}"
                .WithHeaders(new { doc_uuid=docSynch.Uuid})
                .PostJsonAsync(cashMoney);
        }

        public async Task SendNewGood(DocSynch docSynch)
        {
            var good = db.NewGoodsFromCash.Include(n => n.Good)
                .ThenInclude(g=>g.BarCodes)
                .Where(n => n.Id == docSynch.DocId).FirstOrDefault()?.Good;
            var goodSynch = new GoodSynchDataModel
            {
                Uuid = good.Uuid,
                Name = good.Name,
                Unit = good.Unit,
                SpecialType = good.SpecialType,
                VPackage = good.VPackage,
                Price = good.Price,
                IsDeleted = false,
                Barcodes=good.BarCodes.Select(b=>b.Code).ToList()
            };
            await $"{serverurl}/api/onlinecash/NewGoodFromCashSynch/{shopId}"
                .WithHeaders(new { doc_uuid = docSynch.Uuid })
                .PostJsonAsync(goodSynch);
        }

        public async Task SendRevaluation(DocSynch docSynch)
        {
            var revaluation = await db.Revaluations.Include(r => r.RevaluationGoods).ThenInclude(r => r.Good).Where(r => r.Id == docSynch.DocId).FirstOrDefaultAsync();
            RevaluationDataModel dataModel = new RevaluationDataModel { Uuid=revaluation.Uuid, Create = revaluation.Create };
            foreach (var rGood in revaluation.RevaluationGoods)
                dataModel.RevaluationGoods.Add(new RevaluationGoodDataModel
                {
                    Uuid = rGood.Good.Uuid,
                    PriceOld = rGood.PriceOld,
                    PriceNew = rGood.PriceNew
                });
            var resp=await $"{serverurl}/api/onlinecash/revaluationSynch/{shopId}"
                .WithHeaders(new { doc_uuid = docSynch.Uuid })
                .PostJsonAsync(dataModel);
            if (resp.StatusCode == 200)
            {
                string str = await resp.ResponseMessage.Content.ReadAsStringAsync();
                if (str != "")
                {
                    RevaluationDataModel respRevaluation = JsonSerializer.Deserialize<RevaluationDataModel>(str);
                    foreach (var rGood in respRevaluation.RevaluationGoods)
                        revaluation.RevaluationGoods.Where(r => r.Good.Uuid == rGood.Uuid).FirstOrDefault().Count = rGood.Count;
                }
            }
            await db.SaveChangesAsync();
        }
    }

    class CashBoxCheckSellBuyer
    {
        public Guid Uuid { get; set; }
        public string Phone { get; set; }
    }

    class CashBoxCheckSellGood
    {
        public Guid Uuid { get; set; }
        public decimal Count { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
    }

    class CashBoxCheckSellModel
    {
        public DateTime Create { get; set; }
        public bool IsReturn { get; set; } = false;
        public CashBoxCheckSellBuyer Buyer { get; set; }
        public decimal SumCash { get; set; }
        public decimal SumElectron { get; set; }
        public decimal SumDiscount { get; set; }
        public List<CashBoxCheckSellGood> Goods { get; set; } = new List<CashBoxCheckSellGood>();
    }
}
