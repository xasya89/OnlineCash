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
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using System.Threading;
using OnlineCashRmk.Extensions;
using Microsoft.Extensions.Logging;
using OnlineCashRmk.Services;

namespace OnlineCashRmk.ViewModels
{
    public static class SynchWithServerViewModel
    {
        private static int shopId;
        public static async Task StartSynch(string serverUrl, int shop, DataContext db, HttpClient httpClient)
        {
            shopId = shop;
            IEnumerable<DocSynch> docs = await db.DocSynches.Where(d => d.SynchStatus == false).OrderBy(d => d.Create).ToListAsync();
            foreach (var doc in docs)
            {
                if (doc.Uuid.ToString() == "00000000-0000-0000-0000-000000000000")
                    doc.Uuid = Guid.NewGuid();
                httpClient.AddDefaultHeaders(doc.Uuid);
                switch (doc.TypeDoc)
                {
                    case TypeDocs.OpenShift:
                        await SendOpenShift(serverUrl, httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.Buy:
                        await SendCheckSell(serverUrl, httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.CloseShift:
                        await SendCloseShift(serverUrl, httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.WriteOf:
                        await SendWriteOf(serverUrl, httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.Arrival:
                        await SendArrival(serverUrl, httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.StockTaking:
                        await SendStocktaking(serverUrl, httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.StartStocktacking:
                        await SendStartStocktacking(serverUrl, httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.StopStocktacking:
                        await SendStopStocktacking(serverUrl, httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.CashMoney:
                        await SendCashMoney(serverUrl, httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.NewGoodFromCash:
                        await SendNewGood(serverUrl, httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.Revaluation:
                        await SendRevaluation(serverUrl, httpClient, db, doc.DocId);
                        break;
                }
                doc.SynchStatus = true;
                doc.Synch = DateTime.Now;
                await db.SaveChangesAsync();
            }
        }
        /*
        public async Task GetBuyersAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var buyers = await client.GetFromJsonAsync<List<Buyer>>($"{serverurl}/api/onlinecash/Buyers");
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
        */
        public static async Task GetDiscountSettings(string serverurl, HttpClient httpClient, DataContext db)
        {
            var discounts = await httpClient.GetFromJsonAsync<DiscountParamContainerModel>($"{serverurl}/api/onlinecash/Discounts");
            var discountsDb = await db.DiscountSettings.FirstOrDefaultAsync();
            if (discountsDb == null)
            {
                db.DiscountSettings.Add(new DiscountSetting { DiscountModel = discounts });
                await db.SaveChangesAsync();
            }
            else
            {
                discountsDb.DiscountModel = discounts;
            }
        }

        private static async Task SendArrival(string serverurl, HttpClient httpClient, DataContext db, int docId)
        {
            var arrival = await db.Arrivals
                .Include(a => a.ArrivalGoods)
                .ThenInclude(a => a.Good)
                .Where(a => a.Id == docId).FirstOrDefaultAsync();
            if (arrival == null)
                throw new Exception($"Ошиюка отправки arrival is null in database - {docId}");
            var model = new ArrivalSynchDataModel { Num = arrival.Num, DateArrival = arrival.DateArrival, SupplierId = arrival.SupplierId, Uuid = arrival.Uuid };
            foreach (var aGood in arrival.ArrivalGoods)
                model.ArrivalGoods.Add(new ArrivalGoodSynchDataModel
                {
                    GoodUuid = aGood.Good.Uuid,
                    Price = aGood.Price,
                    Count = aGood.Count,
                    Nds = aGood.Nds,
                    ExpiresDate = aGood.ExpiresDate
                });

            var resp = await httpClient.PostAsJsonAsync($"{serverurl}/api/onlinecash/ArrivalSynch/{shopId}", model);
            if (resp.IsSuccessStatusCode)
                throw new Exception($"Ошиюка отправки arrival id - {docId}");

        }

        private static async Task SendOpenShift(string serverurl, HttpClient httpClient, DataContext db, int docId)
        {
            var shift = await db.Shifts.Where(s => s.Id == docId).FirstOrDefaultAsync();
            var resp = await httpClient.PostAsync($"{serverurl}/api/onlinecash/Cashbox/OpenShift/{shopId}/{shift.Uuid}/{shift.Start.ToString()}", null);
            if (!resp.IsSuccessStatusCode)
                throw new Exception("Ошибка отправки на сервер Open shift id - " + docId);
        }

        private static async Task SendCloseShift(string serverurl, HttpClient httpClient, DataContext db, int docId)
        {
            var shiftclose = await db.Shifts.Where(s => s.Id == docId).FirstOrDefaultAsync();
            var resp = await httpClient.PostAsync($"{serverurl}/api/onlinecash/Cashbox/CloseShift/{shiftclose.Uuid}/{shiftclose.Stop.ToString()}", null);
            if (!resp.IsSuccessStatusCode)
                throw new Exception("Ошибка отправки на сервер Close shift id - " + docId);
        }

        public static async Task SendCheckSell(string serverurl, HttpClient httpClient, DataContext db, int docId)
        {
            var sell = db.CheckSells
                .Include(s => s.CheckGoods).ThenInclude(g => g.Good)
                .Include(s => s.CheckPayments)
                .Include(s => s.Buyer)
                .Where(s => s.Id == docId).AsNoTracking().FirstOrDefault();
            var shiftbuy = db.Shifts.Where(s => s.Id == sell.ShiftId).AsNoTracking().FirstOrDefault();
            CashBoxCheckSellBuyer cashBuyer = null;
            if (sell.Buyer != null)
                cashBuyer = new CashBoxCheckSellBuyer { Uuid = sell.Buyer.Uuid, Phone = sell.Buyer.Phone };
            decimal sumCash = sell.CheckPayments.Where(p => p.TypePayment == TypePayment.Cash).Sum(p => p.Sum);
            decimal sumElectron = sell.CheckPayments.Where(p => p.TypePayment == TypePayment.Electron).Sum(p => p.Sum);
            var sellPost = new CashBoxCheckSellModel
            {
                IsReturn = sell.IsReturn,
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

            var resp = await httpClient.PostAsJsonAsync($"{serverurl}/api/onlinecash/CashBox/sell/{shiftbuy.Uuid}", sellPost);
            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Ошиюка отправки sell id - {docId}");
        }

        private static async Task SendWriteOf(string serverurl, HttpClient httpClient, DataContext db, int docId)
        {
            var writeof = await db.Writeofs
                .Include(w => w.WriteofGoods)
                .ThenInclude(wg => wg.Good)
                .Where(w => w.Id == docId).AsNoTracking().FirstOrDefaultAsync();
            List<WriteofGoodSynchDataModel> goods = new List<WriteofGoodSynchDataModel>();
            foreach (var wg in writeof.WriteofGoods)
                goods.Add(new WriteofGoodSynchDataModel { Uuid = wg.Good.Uuid, Count = wg.Count, Price = wg.Price });
            var writeofsynch = new WriteofSynchDataModel { DateCreate = writeof.DateCreate, Note = writeof.Note, Goods = goods };
            var resp = await httpClient.PostAsJsonAsync($"{serverurl}/api/onlinecash/WriteofSynch/{shopId}", writeofsynch);
            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Ошиюка отправки writeof id - {docId}");
        }

        //TODO: Возможно этот метод не нужен, т.к.уже существует методы начал и завершения инверторизации
        public static async Task SendStocktaking(string serverurl, HttpClient httpClient, DataContext db, int docId)
        {
            var stocktaking = await db.Stocktakings.Include(s => s.StocktakingGroups)
                .ThenInclude(s => s.StocktakingGoods).ThenInclude(g => g.Good)
                .Where(s => s.Id == docId).FirstOrDefaultAsync();
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

            var resp = await httpClient.PostAsJsonAsync($"{serverurl}/api/onlinecash/Stocktaking/{shopId}", stocktakingSend);
            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Ошиюка отправки stocktacking id - {docId}");
        }

        public static async Task SendStartStocktacking(string serverurl, HttpClient httpClient, DataContext db, int docId)
        {
            var stocktaking = await db.Stocktakings.Where(s => s.Id == docId).FirstOrDefaultAsync();
            StocktakingSendDataModel model = new StocktakingSendDataModel
            {
                Create = stocktaking.Create,
                Uuid = stocktaking.Uuid,
                CashMoney = stocktaking.CashMoney
            };

            var resp = await httpClient.PostAsJsonAsync($"{serverurl}/api/onlinecash/Stocktaking/start/{shopId}", model);
            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Ошиюка отправки stocktacking start id - {docId}");
        }

        public static async Task SendStopStocktacking(string serverurl, HttpClient httpClient, DataContext db, int docId)
        {
            var stocktaking = await db.Stocktakings.Include(s => s.StocktakingGroups).ThenInclude(s => s.StocktakingGoods).ThenInclude(g => g.Good)
                .Where(s => s.Id == docId).FirstOrDefaultAsync();
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

            var resp = await httpClient.PostAsJsonAsync($"{serverurl}/api/onlinecash/Stocktaking/stop/{stocktaking.Uuid}", groups);
            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Ошиюка отправки stocktacking stop id - {docId}");
        }

        public static async Task SendCashMoney(string serverurl, HttpClient httpClient, DataContext db, int docId)
        {
            var cashMoney = db.CashMoneys.Where(c => c.Id == docId).FirstOrDefault();
            if (cashMoney == null)
                throw new Exception("Не найден документ cashMoney");

            var resp = await httpClient.PostAsJsonAsync($"{serverurl}/api/onlinecash/CashMoneys/{shopId}", cashMoney);
            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Ошиюка отправки cashmoney id - {docId}");
        }

        public static async Task SendNewGood(string serverurl, HttpClient httpClient, DataContext db, int docId)
        {
            var good = db.NewGoodsFromCash.Include(n => n.Good)
                .ThenInclude(g => g.BarCodes)
                .Where(n => n.Id == docId).FirstOrDefault()?.Good;
            var goodSynch = new GoodSynchDataModel
            {
                Uuid = good.Uuid,
                Name = good.Name,
                Unit = good.Unit,
                SpecialType = good.SpecialType,
                VPackage = good.VPackage,
                Price = good.Price,
                IsDeleted = false,
                Barcodes = good.BarCodes.Select(b => b.Code).ToList()
            };
            var resp = await httpClient.PostAsJsonAsync($"{serverurl}/api/onlinecash/NewGoodFromCashSynch/{shopId}", goodSynch);
            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Ошиюка отправки new good id - {docId}");
        }

        public static async Task SendRevaluation(string serverurl, HttpClient httpClient, DataContext db, int docId)
        {
            var revaluation = await db.Revaluations
                .Include(r => r.RevaluationGoods)
                .ThenInclude(r => r.Good)
                .Where(r => r.Id == docId).FirstOrDefaultAsync();
            RevaluationDataModel dataModel = new RevaluationDataModel { Uuid = revaluation.Uuid, Create = revaluation.Create };
            foreach (var rGood in revaluation.RevaluationGoods)
                dataModel.RevaluationGoods.Add(new RevaluationGoodDataModel
                {
                    Uuid = rGood.Good.Uuid,
                    PriceOld = rGood.PriceOld,
                    PriceNew = rGood.PriceNew
                });
            var resp = await httpClient.PostAsJsonAsync($"{serverurl}/api/onlinecash/revaluationSynch/{shopId}", dataModel);
            if (resp.IsSuccessStatusCode)
            {
                string str = await resp.Content.ReadAsStringAsync();
                if (str != "")
                {
                    RevaluationDataModel respRevaluation = JsonSerializer.Deserialize<RevaluationDataModel>(str);
                    foreach (var rGood in respRevaluation.RevaluationGoods)
                        revaluation.RevaluationGoods.Where(r => r.Good.Uuid == rGood.Uuid).FirstOrDefault().Count = rGood.Count;
                }
            }
            else
                throw new Exception($"Ошиюка отправки revaluation id - {docId}");

        }
    }
}
