using OnlineCash.Models.StockTackingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Models;
using OnlineCash.Models.StockTackingModels;
using OnlineCash.DataBaseModels;
using OnlineCash.Services;

namespace OnlineCash.Services
{
    public class StockTackingService : IStockTackingService
    {
        shopContext db;
        IGoodBalanceService _goodBalanceService;
        CashMoneyService _moneyService;
        NotificationOfEventInSystemService _notificationService;
        MoneyBalanceService _moneyBalanceService;
        GoodCountBalanceService _countBalanceService;
        public StockTackingService(shopContext db, 
            IGoodBalanceService goodBalanceService, 
            CashMoneyService moneyService, 
            NotificationOfEventInSystemService notificationService,
            MoneyBalanceService moneyBalanceService,
            GoodCountBalanceService countBalanceService)
        {
            this.db = db;
            _goodBalanceService = goodBalanceService;
            _moneyService = moneyService;
            _notificationService = notificationService;
            _moneyBalanceService = moneyBalanceService;
            _countBalanceService = countBalanceService;
        }

        public async Task<StockTackingGruppingModel> GetDetailsGroups(int idStocktaking)
        {
            var stocktacking = await db.Stocktakings.Include(st => st.StockTakingGroups)
                .ThenInclude(gr => gr.StocktakingGoods)
                .ThenInclude(g => g.Good)
                .Where(st => st.Id == idStocktaking).FirstOrDefaultAsync();
            var shop = await db.Shops.Where(s => s.Id == stocktacking.ShopId).FirstOrDefaultAsync();
            StockTackingGruppingModel model = new StockTackingGruppingModel {NumDoc=stocktacking.Num, DateDoc=stocktacking.Create, ShopName=shop.Name };
            var goods = await db.Goods.ToListAsync();
            foreach(var group in stocktacking.StockTakingGroups)
                foreach(var good in group.StocktakingGoods)
                {
                    var groupModel = model.Groups.Where(gr => gr.GroupId == good.Good.GoodGroupId).FirstOrDefault();
                    if(groupModel==null)
                    {
                        var gr = db.GoodGroups.Where(gr => gr.Id == good.Good.GoodGroupId).FirstOrDefault();
                        groupModel = new StockTackingGruppingGroupModel { GroupId = gr.Id, GroupName = gr.Name };
                        model.Groups.Add(groupModel);
                    };
                    var goodModel = groupModel.Goods.Where(g => g.GoodId == good.GoodId).FirstOrDefault();
                    if (goodModel == null)
                    {
                        goodModel = new StockTackingGruppingGoodModel { 
                            GoodId = good.GoodId, 
                            GoodName = good.Good.Name, 
                            CountDb = Math.Round(good.CountDB, 3), 
                            CountFact = Math.Round(good.CountFact, 3), 
                            Price = good.Price 
                        };
                        groupModel.Goods.Add(goodModel);
                    }
                    else
                    {
                        goodModel.CountDb += Math.Round(good.CountDB,3);
                        goodModel.CountFact += Math.Round(good.CountFact,3);
                        goodModel.Price = good.Price;
                    }
                }
            return model;
        }

        public async Task<StocktackingSummaryModel> GetSummary(int stocktakingId, int? goodGroupdId)
        {
            var stocktaking= await db.Stocktakings
                .Include(s => s.StocktakingSummaryGoods)
                .ThenInclude(g => g.Good).Where(s => s.Id == stocktakingId).FirstOrDefaultAsync();

            var stocktackingLateCount = await db.Stocktakings.Where(s => s.Id > stocktakingId).CountAsync();

            StocktackingSummaryModel model = new StocktackingSummaryModel
            {
                Id=stocktaking.Id,
                isEditable=stocktackingLateCount==0,
                Start = stocktaking.Start,
                SumDb = stocktaking.SumDb,
                SumFact = stocktaking.SumFact,
                CashMoneyDb = stocktaking.CashMoneyDb,
                CashMoneyFact = stocktaking.CashMoneyFact
            };

            var stocktackingOld = await db.Stocktakings.Include(s => s.StocktakingSummaryGoods).Where(s => s.Start < stocktaking.Start).OrderBy(s => s.Start).LastOrDefaultAsync();
            Dictionary<int, decimal> summaryOldDict = new Dictionary<int, decimal>();
            if (stocktackingOld != null)
                foreach (var summaryOld in stocktackingOld.StocktakingSummaryGoods)
                    summaryOldDict.Add(summaryOld.GoodId, summaryOld.CountFact);

            foreach (var summary in stocktaking.StocktakingSummaryGoods)
                if(goodGroupdId==null || summary.Good.GoodGroupId==goodGroupdId)
                model.Goods.Add(new StocktackingSummaryGoodModel
                {
                    GoodId = summary.GoodId,
                    GoodName = summary.Good.Name,
                    Unit=summary.Good.Unit,
                    Price = summary.Price,
                    CountLast = summaryOldDict.ContainsKey(summary.GoodId) ? summaryOldDict[summary.GoodId] : 0,
                    CountDb=summary.CountDb,
                    CountFact=summary.CountFact
                });
            return model;
        }

        /// <summary>
        /// Изменение обощенных данных по инверторизации
        /// </summary>
        public async Task ChangeSummary(int stocktackingId, List<StocktackingSummaryGoodModel> model)
        {
            foreach (var summary in model)
            {
                var dbSummary= await db.StocktakingSummaryGoods.Where(s => s.StocktakingId == stocktackingId & s.GoodId == summary.GoodId).FirstOrDefaultAsync();
                if(dbSummary!=null)
                    dbSummary.CountFact = summary.CountFact;
            }
                
            await db.SaveChangesAsync();
            foreach (var summary in model)
                await _countBalanceService.Change(stocktackingId, summary);
        }

        /// <summary>
        /// Получение инверторизации через api кассы. Начало записть текущих остатков из бд и суммы денег в кассе
        /// </summary>
        public async Task StartFromOnlineCash(int shopId, StocktakingReciveDataModel model)
        {

            decimal sumMoneyDb = (await _moneyBalanceService.GetToday(shopId, model.Create.AddDays(0).Date)).SumEnd;
            int num = await db.Stocktakings.CountAsync();
            var stocktaking = new Stocktaking
            {

                Create = model.Create,
                Start = model.Create,
                Uuid = model.Uuid,
                Num = num + 1,
                ShopId = shopId,
                Status = DocumentStatus.New,
                isSuccess = false,
                CashMoneyDb = sumMoneyDb,
                CashMoneyFact = model.CashMoney
            };
            db.Stocktakings.Add(stocktaking);
            var goods = await db.Goods.Include(g => g.GoodPrices).ToListAsync();
            var countsCurrent = await db.GoodCountBalanceCurrents.ToListAsync();
            decimal sumDb = 0;
            foreach (var current in countsCurrent)
                if (countsCurrent.Count != 0)
                {
                    var good = goods.Where(g => g.Id == current.GoodId).FirstOrDefault();
                    decimal price = good.GoodPrices.Where(p => p.ShopId == shopId).FirstOrDefault()?.Price ?? 0;
                    db.StocktakingSummaryGoods.Add(new StocktakingSummaryGood
                    {
                        Stocktaking = stocktaking,
                        GoodId = current.GoodId,
                        Price = price,
                        CountDb = current.Count
                    });
                    sumDb += price * current.Count;
                }
            stocktaking.SumDb = sumDb;
            await db.SaveChangesAsync();

            //Установим деньги
            await _moneyBalanceService.SetStocktacking(stocktaking.ShopId, model.CashMoney);
        }

        /// <summary>
        /// Завершение инверторизации по кассе запись фактических остков и их анализ
        /// </summary>
        public async Task StopFromOnlineCash(Guid uuid, List<StocktakingGroupReciveDataModel> groups)
        {
            var stocktacking = await db.Stocktakings.Where(s => s.Uuid == uuid).FirstOrDefaultAsync();
            if (stocktacking == null)
                throw new Exception($"Не найдена инверторизация с uuid {uuid}");
            stocktacking.isSuccess = true;
            stocktacking.Status = DocumentStatus.Confirm;
            var goods = await db.Goods.Include(g => g.GoodPrices).ToListAsync();
            foreach (var group in groups)
                foreach (var good in group.Goods)
                    if (goods.Where(g => g.Uuid == good.Uuid).FirstOrDefault() == null)
                        throw new Exception($"Товар с uuid - {good.Uuid} не существует");

            Dictionary<int, decimal> summary = new Dictionary<int, decimal>();
            foreach (var group in groups)
            {
                var groupDb = new StockTakingGroup { Stocktaking = stocktacking, Name = group.Name };
                db.StockTakingGroups.Add(groupDb);
                foreach (var good in group.Goods)
                {
                    var gooddb = goods.Where(g => g.Uuid == good.Uuid).FirstOrDefault();
                    var pricedb = gooddb.GoodPrices.Where(p => p.ShopId == stocktacking.ShopId).FirstOrDefault().Price;
                    db.StocktakingGoods.Add(new StocktakingGood { 
                        StockTakingGroup = groupDb, GoodId = gooddb.Id, Price = pricedb, CountFact = good.CountFact 
                    });
                    if (summary.ContainsKey(gooddb.Id))
                        summary[gooddb.Id] += (decimal)good.CountFact;
                    else
                        summary.Add(gooddb.Id, (decimal)good.CountFact);
                }
            }
            var summaryDbList = await db.StocktakingSummaryGoods.Where(s => s.StocktakingId == stocktacking.Id).ToListAsync();
            foreach (KeyValuePair<int, decimal> su in summary)
            {
                var summaryDb = summaryDbList.Where(s => s.GoodId == su.Key).FirstOrDefault();
                if (summaryDb != null)
                    summaryDb.CountFact = su.Value;
                else
                {
                    summaryDb = new StocktakingSummaryGood
                    {
                        StocktakingId=stocktacking.Id,
                        GoodId = su.Key,
                        Price = goods.Where(g => g.Id == su.Key).FirstOrDefault().GoodPrices.Where(g => g.ShopId == stocktacking.ShopId).FirstOrDefault()?.Price ?? 0,
                        CountDb = 0,
                        CountFact = Math.Round( su.Value,3)
                    };
                    db.StocktakingSummaryGoods.Add(summaryDb);
                }
                stocktacking.SumFact += summaryDb.CountFact * summaryDb.Price;
            }

            await db.SaveChangesAsync();
            await _countBalanceService.Add<StocktakingSummaryGood>(
                stocktacking.Id, 
                stocktacking.Start, 
                await db.StocktakingSummaryGoods.Where(s => s.StocktakingId == stocktacking.Id).ToListAsync()
                );
            await CreateReportAfterSave(stocktacking.Id);
        }
        public async Task SaveFromOnlinCash(int shopId, StocktakingReciveDataModel model)
        {
            var goods = await db.Goods.Include(g => g.GoodPrices).ToListAsync();
            foreach (var group in model.Groups)
                foreach (var good in group.Goods)
                    if (goods.Where(g => g.Uuid == good.Uuid).FirstOrDefault() == null)
                        throw new Exception($"Товар с uuid - {good.Uuid} не существует");
            var num = await db.Stocktakings.CountAsync();
            var stocktaking = new Stocktaking {
                Create = model.Create,
                Start = model.Create,
                Num =num + 1,
                ShopId = shopId,
                Status = DocumentStatus.Confirm,
                isSuccess = true
            };
            db.Stocktakings.Add(stocktaking);
            foreach (var group in model.Groups)
            {
                var groupDb = new StockTakingGroup { Stocktaking = stocktaking, Name = group.Name };
                db.StockTakingGroups.Add(groupDb);
                foreach (var good in group.Goods)
                {
                    var gooddb = goods.Where(g => g.Uuid == good.Uuid).FirstOrDefault();
                    var pricedb = gooddb.GoodPrices.Where(p => p.ShopId == shopId).FirstOrDefault().Price;
                    db.StocktakingGoods.Add(new StocktakingGood { StockTakingGroup = groupDb, GoodId = gooddb.Id, Price = pricedb, CountFact = good.CountFact });
                }
            }
            
            await db.SaveChangesAsync();
            await SuccessCalc(stocktaking.Id);
            await CreateReportAfterSave(stocktaking.Id);
        }

        public async Task Save(StockTakingSaveModel model)
        {
            Stocktaking stocktaking = await db.Stocktakings.Include(s => s.StockTakingGroups).ThenInclude(gr => gr.StocktakingGoods).Where(s => s.Id == model.id).FirstOrDefaultAsync();
            bool isSuccessOld = stocktaking.isSuccess;
            stocktaking.isSuccess = model.isSuccess;
            stocktaking.Status = model.isSuccess ? DocumentStatus.Confirm : stocktaking.Status;
            foreach (var sgr in model.Groups)
            {
                StockTakingGroup group = null;
                if (sgr.Id == -1)
                {
                    group = new StockTakingGroup
                    {
                        Name = sgr.Name,
                        Stocktaking = stocktaking
                    };
                    db.StockTakingGroups.Add(group);
                }
                else
                {
                    group = await db.StockTakingGroups.Where(gr => gr.Id == sgr.Id).FirstOrDefaultAsync();
                    if (group == null)
                        throw new Exception("Не найден идентификатор группы");
                    group.Name = sgr.Name;
                };
                //Добавим новые товары
                foreach (var sg in sgr.Goods.Where(g => g.id == -1).ToList())
                {
                    var good = await db.Goods.Include(g => g.GoodPrices).Where(g => g.Id == sg.idGood).FirstOrDefaultAsync();
                    StocktakingGood stocktakingGood = new StocktakingGood
                    {
                        StockTakingGroup = group,
                        Good = good,
                        Count = 0,
                        CountDB = 0,
                        CountFact = sg.CountFact,
                        Price = good.GoodPrices.Where(p => p.ShopId == stocktaking.ShopId).FirstOrDefault().Price
                    };
                    db.StocktakingGoods.Add(stocktakingGood);
                }
                //Изменим количество
                foreach (var sg in sgr.Goods.Where(g => g.id != -1).ToList())
                {
                    StocktakingGood stocktakingGood = await db.StocktakingGoods.Where(g => g.Id == sg.id).FirstOrDefaultAsync();
                    stocktakingGood.CountFact = sg.CountFact;
                }
            }
            await db.SaveChangesAsync();
            if (model.isSuccess & !isSuccessOld)
            {
                await SuccessCalc(stocktaking.Id);
                await CreateReportAfterSave(stocktaking.Id);
            }
        }

        private async Task SuccessCalc(int stocktakingId)
        {
            var stocktaking = await db.Stocktakings.Include(s=>s.StockTakingGroups).ThenInclude(gr=>gr.StocktakingGoods).Where(s => s.Id == stocktakingId).FirstOrDefaultAsync();
            List<StocktakingSummaryGood> summaryes = new List<StocktakingSummaryGood>();
            var balances = await db.GoodBalances.ToListAsync();
            foreach (var group in stocktaking.StockTakingGroups)
                foreach (var good in group.StocktakingGoods)
                {
                    var summary = summaryes.Where(s => s.GoodId == good.GoodId).FirstOrDefault();
                    decimal countDb = 0;
                    var goodBalance = balances.Where(b => b.GoodId == good.GoodId).FirstOrDefault();
                    if (goodBalance != null)
                        countDb = (decimal)goodBalance.Count;
                    if (summary == null)
                        summaryes.Add(new StocktakingSummaryGood
                        {
                            Stocktaking = stocktaking,
                            GoodId = good.GoodId,
                            CountFact = (decimal)good.CountFact,
                            CountDb = countDb,
                            Price = good.Price
                        });
                    else
                    {
                        summary.CountDb += (decimal)countDb;
                        summary.CountFact += (decimal)good.CountFact;
                    }
                }
            //Найдем не указанные в инверторизации товары и добавим их в нераспределенные
            var goodNotAdded = new List<StocktakingGood>();

            
            var goods = await db.Goods.Include(g=>g.GoodPrices.Where(s=>s.ShopId==stocktaking.ShopId)).Where(g => g.IsDeleted == false).ToListAsync();
            var groupNoFind = new StockTakingGroup { Stocktaking = stocktaking, Name = "Не указан в инверторизации" };
            foreach (var balance in balances)
                if (balance.Count != 0)
                {
                    var find = false;
                    var good = goods.Where(g => g.Id == balance.GoodId).FirstOrDefault();
                    var pricedb = good?.GoodPrices.FirstOrDefault().Price;
                    if (good != null)
                    {
                        foreach (var stGroup in stocktaking.StockTakingGroups)
                            foreach (var stGood in stGroup.StocktakingGoods)
                                if (stGood.GoodId == balance.Good.Id)
                                    find = true;
                        if (!find)
                        {
                            goodNotAdded.Add(new StocktakingGood { StockTakingGroup = groupNoFind, GoodId = good.Id, CountDB = balance.Count, CountFact = 0 });
                            summaryes.Add(new StocktakingSummaryGood
                            {
                                Stocktaking = stocktaking,
                                GoodId = good.Id,
                                Price = (decimal)pricedb,
                                CountDb = (decimal)balance.Count,
                                CountFact = 0
                            });
                        }
                    }
                };
            if(goodNotAdded.Count>0)
            {
                db.StockTakingGroups.Add(groupNoFind);
                db.StocktakingGoods.AddRange(goodNotAdded);
            };
            db.StocktakingSummaryGoods.AddRange(summaryes);

            //запишем итоги
            stocktaking.SumDb = summaryes.Sum(s => s.Price * s.CountDb);
            stocktaking.SumFact = summaryes.Sum(s => s.Price * s.CountFact);

            await db.SaveChangesAsync();
            await _goodBalanceService.CalcAsync(stocktaking.ShopId, stocktaking.Start);
        }

        public async Task CreateReportAfterSave(int stocktakingId)
        {
            var stocktaking = await db.Stocktakings.Where(s => s.Id == stocktakingId).FirstOrDefaultAsync();
            int stocktackingOldId = stocktakingId;
            Stocktaking stocktakingOld = null;
            while (--stocktackingOldId > 0 & stocktakingOld == null)
                stocktakingOld = await db.Stocktakings.Where(s => s.Id == stocktackingOldId).Include(s=>s.ReportsAfterStocktaking).FirstOrDefaultAsync();
            decimal stocktakingOldSum = stocktakingOld==null ? 0 : stocktakingOld.SumFact;
            DateTime startDate = stocktakingOld==null ? DateTime.Now.AddDays(-7).Date : stocktakingOld.Start.Date;
            DateTime stopDate = stocktaking.Start.AddDays(-1).Date;
            decimal cashMoneyOld = 0;
            if ((stocktakingOld?.ReportsAfterStocktaking.FirstOrDefault() ?? null) != null)
                cashMoneyOld = stocktakingOld.ReportsAfterStocktaking.FirstOrDefault().CashStartSum;
            //Приходы
            decimal sumArrival = 0;
            var arrivals = await db.Arrivals.Where(a => a.isSuccess==true & a.DateArrival.Date >= startDate & a.DateArrival.Date <= stopDate).ToListAsync();
            foreach (var arrival in arrivals)
                sumArrival += arrival.SumArrival;
            //Выплаты
            decimal sumOutcome = 0;
            var outcomes = await db.CashMoneys.Where(c => c.TypeOperation == CashMoneyTypeOperations.Outcome & c.Create.Date >= startDate & c.Create.Date <= stopDate).ToListAsync();
            foreach (var outcome in outcomes)
                sumOutcome += outcome.Sum;
            //Терминал
            decimal sumDiscount = 0;
            decimal sumElectron = 0;
            var shifts = await db.Shifts.Where(s => s.Start.Date >= startDate & s.Start.Date <= stopDate).ToListAsync();
            foreach (var shift in shifts)
            {
                sumDiscount += shift.SumDiscount;
                sumElectron += shift.SumElectron;
            }
            //Списания
            decimal sumWritof = 0;
            var writeofs = await db.Writeofs.Where(w => w.IsSuccess == true & w.DateWriteof.Date >= startDate & w.DateWriteof.Date <= stopDate).ToListAsync();
            foreach (var writeof in writeofs)
                sumWritof += writeof.SumAll;
            //Переоценка
            decimal revaluationWriteOf = 0;
            decimal revaluationArrival = 0;
            var revaluations = await db.Revaluations.Where(r => r.Create >= startDate & r.Create <= stopDate).ToListAsync();
            foreach(var revaluation in revaluations)
            {
                revaluationWriteOf += revaluation.SumOld;
                revaluationArrival += revaluation.SumNew;
            }
            //Остаток денег в кассе
            var cashMoneyBalance = await db.MoneyBalanceHistories.Where(m => m.DateBalance.Date == stopDate).FirstOrDefaultAsync();
            decimal cashMoneyEnd = cashMoneyBalance == null ? 0 : cashMoneyBalance.SumEnd;
            ReportAfterStocktaking reportAfterStocktaking = new ReportAfterStocktaking
            {
                Stocktaking=stocktaking,
                StartDate = startDate,
                StopDate = stopDate,
                StocktakingPrependSum=stocktakingOldSum,
                CashStartSum=cashMoneyOld,
                SumDiscount=sumDiscount,
                ArrivalSum=sumArrival,
                IncomeSum=sumOutcome,
                ElectronSum=sumElectron,
                WriteOfSum=sumWritof,
                RevaluationWriteOf=revaluationWriteOf,
                RevaluationArrival=revaluationArrival,
                CashEndSum= cashMoneyEnd,
                StocktakingFactSum=stocktaking.SumFact
            };
            db.ReportsAfterStocktaking.Add(reportAfterStocktaking);
            await db.SaveChangesAsync();
            await _notificationService.Send($@"Итоги инверторизации за период {startDate.ToString("dd.MM")} - {stopDate.ToString("dd.MM")}:
Иверторизация было - {stocktakingOldSum}
Было денег в кассе - {cashMoneyOld}
Приход - {sumArrival}
Выплаты - {sumOutcome}
Терминал - {sumElectron}
Списание - {sumWritof}
Переоценка (минус) - {revaluationWriteOf}
Переоценка (плюс) - {revaluationArrival}

Должны быть остатки по документам - {reportAfterStocktaking.StopSum}

Факт. инверторизация - {stocktaking.SumFact}
Денег в кассе - {cashMoneyEnd}
");
        }

        public async Task<List<dynamic>> GetDetailGoodCountByDocs(int stocktakingId, int goodId)
        {
            List<dynamic> documents = new List<dynamic>();
            decimal countItog = 0;
            var stocktaking = await db.Stocktakings.Where(s => s.Id == stocktakingId).FirstOrDefaultAsync();
            DateTime stopDate = stocktaking.Start.Date;
            int stocktakingOldId = stocktakingId;
            Stocktaking stocktakingOld = await db.Stocktakings.Include(s=>s.StocktakingSummaryGoods)
                .Where(s=>s.Id< stocktakingId & s.Status==DocumentStatus.Confirm).OrderByDescending(s=>s.Id).FirstOrDefaultAsync();
            DateTime startDate = stocktakingOld?.Start.Date ?? DateTime.Now.AddDays(-7).Date;
            if (stocktakingOld != null)
            {
                double count = (double)stocktakingOld.StocktakingSummaryGoods.Where(s => s.GoodId == goodId).Sum(s => s.CountFact);
                documents.Add(new { Type = "Предыдущая инверторизация", Num = "", Count = count });
                countItog = (decimal)count;
            }
            var arrivals=await db.Arrivals.Include(a=>a.ArrivalGoods).Where(a => a.isSuccess == true & a.DateArrival.Date >= startDate & a.DateArrival.Date < stopDate).ToListAsync();
            foreach(var arrival in arrivals)
            {
                decimal count = arrival.ArrivalGoods.Where(g => g.GoodId == goodId).Sum(a => a.Count);
                if (count > 0)
                    documents.Add(new { Type = "Приход", Num = $"{arrival.Num} от {arrival.DateArrival.ToString("dd.MM")}", Count = $"+ {count}" });
                countItog += (decimal)count;
            };

            var shifts = await db.Shifts.Include(s=>s.ShiftSales).Where(s => s.Start.Date >= startDate & s.Start.Date < stopDate).ToListAsync();
            foreach(var shift in shifts)
            {
                double count = shift.ShiftSales.Where(s => s.GoodId == goodId).Sum(s => s.Count - (double)s.CountReturn);
                if (count > 0)
                    documents.Add(new { Type = "Смена", Num = $"{shift.Start.ToString("dd.MM")}", Count = $"- {count}" });
                countItog -= (decimal)count;
            }

            var writeofs = await db.Writeofs.Include(w=>w.WriteofGoods).Where(w => w.IsSuccess == true & w.DateWriteof.Date >= startDate & w.DateWriteof.Date < stopDate).ToListAsync();
            foreach(var writeof in writeofs)
            {
                decimal count = writeof.WriteofGoods.Where(w => w.GoodId == goodId).Sum(w => w.Count);
                if (count > 0)
                    documents.Add(new { Type = "Списание", Num = $"{writeof.DateWriteof.ToString("dd.MM")}", Count = $"- {count}" });
                countItog -= (decimal)count;
            }

            documents.Add(new { Type = "Итог", Num = $"", Count = $"{countItog}" });
            return documents;
        }
    }

    public class StocktakingReciveDataModel
    {
        public DateTime Create { get; set; }
        public Guid Uuid { get; set; }
        public decimal CashMoney { get; set; }
        public List<StocktakingGroupReciveDataModel> Groups { get; set; } = new List<StocktakingGroupReciveDataModel>();
    }

    public class StocktakingGroupReciveDataModel
    {
        public string Name { get; set; }
        public List<StocktakingGoodReciveDataModel> Goods { get; set; } = new List<StocktakingGoodReciveDataModel>();
    }
    public class StocktakingGoodReciveDataModel
    {
        public Guid Uuid { get; set; }
        public double CountFact { get; set; }
    }
}
