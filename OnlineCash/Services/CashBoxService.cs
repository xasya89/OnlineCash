
using OnlineCash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using OnlineCash.Models.CashBox;
using DatabaseBuyer;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace OnlineCash.Services
{
    public class CashBoxService : ICashBoxService
    {
        shopContext db;
        IConfiguration _configuration;
        shopbuyerContext _dbBuyer;
        CashMoneyService _moneyService;
        MoneyBalanceService _moneyBalanceService;
        GoodCountBalanceService _countBalanceService;
        NotificationOfEventInSystemService _notificationService;
        ILogger<CashBoxService> _logger;
        public CashBoxService(shopContext db,
            IConfiguration configuration,
            shopbuyerContext dbBuyer,
            MoneyBalanceService moneyBalanceService, 
            CashMoneyService moneyService,
            GoodCountBalanceService countBalanceService,
            NotificationOfEventInSystemService notificationService,
            ILogger<CashBoxService> logger)
        {
            this.db = db;
            _configuration = configuration;
            _dbBuyer = dbBuyer;
            _moneyService = moneyService;
            _moneyBalanceService = moneyBalanceService;
            _countBalanceService = countBalanceService;
            _notificationService = notificationService;
            _logger= logger;
        }
        //TODO: Возможно данный метод не нужен
        public async Task<bool> Buy(Guid uuid, List<CashBoxBuyReturnModel> buylist)
        {
            var shift = await db.Shifts.Where(s => s.Uuid == uuid & s.Stop == null).FirstOrDefaultAsync();
            if (shift == null)
                return false;
            var goods = db.Goods.ToList();
            foreach (var buy in buylist)
            {
                var good = goods.Where(g => g.Uuid == buy.Uuid).FirstOrDefault();
                if (good == null)
                    return false;
                var sale = await db.ShiftSales.Where(s => s.ShiftId == shift.Id & s.GoodId == good.Id & s.Price == buy.Price).FirstOrDefaultAsync();
                if (sale != null)
                    sale.Count += buy.Count;
                else
                    db.ShiftSales.Add(new ShiftSale
                    {
                        Shift = shift,
                        Good = good,
                        Price = buy.Price,
                        Count = buy.Count
                    });
            }
            shift.SumSell += buylist.Sum(b => (decimal)b.Count * b.Price);
            shift.SumElectron+=buylist.Where(b=>b.IsElectron).Sum(b => (decimal)b.Count * b.Price);
            shift.SumNoElectron+=buylist.Where(b=>b.IsElectron==false).Sum(b => (decimal)b.Count * b.Price);
            shift.SumAll+= buylist.Sum(b => (decimal)b.Count * b.Price);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task Sell(Guid uuid, CashBoxCheckSellModel check)
        {
            var shift = await db.Shifts.Where(s => s.Uuid == uuid & s.Stop == null).FirstOrDefaultAsync();
            if (shift == null)
                throw new Exception($"Смена с uuid {uuid} не найдена");
            var goods = await db.Goods.ToListAsync();
            foreach (var good in check.Goods)
                if (goods.Where(g => g.Uuid == good.Uuid).FirstOrDefault() == null)
                    throw new Exception($"Товар с uuid {good.Uuid} не найден");
            foreach(var checkGood in check.Goods)
            {
                var good = goods.Where(g => g.Uuid == checkGood.Uuid).FirstOrDefault();
                var sale = await db.ShiftSales.Where(s => s.ShiftId == shift.Id & s.GoodId == good.Id & s.Price == checkGood.Price).FirstOrDefaultAsync();
                if (check.IsReturn == false)
                {
                    if (sale != null)
                        sale.Count += (double)checkGood.Count;
                    else
                        db.ShiftSales.Add(new ShiftSale
                        {
                            Shift = shift,
                            Good = good,
                            Price = checkGood.Price,
                            Count = (double)checkGood.Count
                        });
                    await _countBalanceService.AddSell(shift.Id, shift.Start, good.Id, checkGood.Count);
                }
                if (check.IsReturn == true)
                {
                    if (sale != null)
                        sale.CountReturn += checkGood.Count;
                    else
                        db.ShiftSales.Add(new ShiftSale
                        {
                            Shift = shift,
                            Good = good,
                            PriceReturn = checkGood.Price,
                            CountReturn = checkGood.Count
                        });
                    await _countBalanceService.AddReturn(shift.Id, shift.Start, good.Id, checkGood.Count);
                }
            }
            if (check.IsReturn == false)
            {
                shift.SumSell += check.SumCash + check.SumElectron; //- check.SumDiscount;
                shift.SumElectron += check.SumElectron;
                shift.SumNoElectron += check.SumCash;
                shift.SumDiscount += check.SumDiscount;
                shift.SumAll += check.SumCash + check.SumElectron; //- check.SumDiscount;
            }
            if (check.IsReturn == true)
            {
                shift.SumReturnElectron += check.SumElectron;
                shift.SumReturnCash += check.SumCash;
                shift.SumAll -= check.SumCash + check.SumElectron;
            }
            //Добавим чек
            Buyer buyer = null;
            if (check.Buyer != null)
            {
                buyer = await _dbBuyer.Buyers.Where(b => b.Uuid == check.Buyer.Uuid).FirstOrDefaultAsync();
                if (buyer == null)
                {
                    buyer = new Buyer
                    {
                        Uuid = check.Buyer.Uuid,
                        Phone = check.Buyer.Phone
                    };
                    _dbBuyer.Buyers.Add(buyer);
                }
                buyer.SumBuy += check.IsReturn || check.SumDiscount > 0 ? 0 : check.SumCash + check.SumElectron;
                buyer.DiscountSum -= buyer.SpecialPercent > 0 ? 0 : check.SumDiscount;
                // Если в чеке есть скидка, то скорее всего она временнная
                // тогда мы обнулим временный процент, т.к. мы уже им воспользовались
                if (check.SumDiscount > 0 & buyer.TemporyPercent > 0)
                    buyer.TemporyPercent = 0;
                // Расчитаем возможные баллы или врменые скидки
                var settingStr = (await _dbBuyer.DiscountSettings.FirstOrDefaultAsync())?.Settings;
                if (settingStr != null)
                    try
                    {
                        var discounts = JsonSerializer.Deserialize<DiscountParamContainerModel>(settingStr);
                        buyer.DiscountSum += check.IsReturn || check.SumDiscount > 0 ? 0 : (check.SumCash + check.SumElectron) * (discounts.PercentFromSale ?? 0) / 100;
                    }
                    catch (Exception) { };
            }

            var checksell = new CheckSell { 
                DateCreate=check.Create,
                TypeSell = check.IsReturn ? TypeSell.Return : TypeSell.Sell,
                Shift=shift,
                BuyerId=buyer?.Id,
                BuyerName=buyer?.Name,
                BuyerPhone=buyer?.Phone,
                SumCash=check.SumCash,
                SumElectron=check.SumElectron,
                Sum=check.SumCash + check.SumElectron,
                SumDiscont=check.SumDiscount,
                SumAll= check.SumCash + check.SumElectron //- check.SumDiscount
            };
            db.CheckSells.Add(checksell);
            foreach(var checkGood in check.Goods)
            {
                var good = goods.Where(g => g.Uuid == checkGood.Uuid).FirstOrDefault();
                db.CheckGoods.Add(new CheckGood
                {
                    CheckSell = checksell,
                    Good = good,
                    Count = checkGood.Count,
                    Price = checkGood.Price
                });
            }

            await db.SaveChangesAsync();
            await _dbBuyer.SaveChangesAsync();
            if (check.SumCash > 0 && check.IsReturn==false)
                await _moneyBalanceService.AddSale(shift.ShopId, check.SumCash);
            if (check.SumCash > 0 && check.IsReturn == true)
                await _moneyBalanceService.AddReturn(shift.ShopId, check.SumCash);
        }

        public async Task<Shift> CloseShift(Guid uuid, DateTime stop)
        {
            var shift = await db.Shifts.Where(s => s.Uuid==uuid & s.Stop == null).FirstOrDefaultAsync();
            if (shift == null)
                throw new Exception($"Смена uuid - {uuid} не найдена или закрыта");
            shift.Stop = stop;
            await db.SaveChangesAsync();
            await _moneyService.Add(shift.ShopId, new CashMoney {
                Create = stop,
                TypeOperation = CashMoneyTypeOperations.Sale,
                Sum = shift.SumNoElectron,
                Note = $"Смена № {shift.Id} от {shift.Start.ToString("dd.MM.yy")}"
            });
            if(shift.SumReturnCash>0)
                await _moneyService.Add(shift.ShopId, new CashMoney
                {
                    Create = stop,
                    TypeOperation = CashMoneyTypeOperations.Return,
                    Sum = shift.SumReturnCash,
                    Note = $"Смена возврат № {shift.Id} от {shift.Start.ToString("dd.MM.yy")}"
                });
            if(_configuration.GetSection("ReportAfterCloseShift").Value=="1")
                await ReportAfterCloseShift(shift);

            return shift;
        }

        async Task ReportAfterCloseShift(Shift shift)
        {
            Stocktaking stocktakingOld = await db.Stocktakings
                .Include(s=>s.ReportsAfterStocktaking).OrderBy(s=>s.Start)
                .LastOrDefaultAsync();
            decimal stocktakingOldSum = stocktakingOld == null ? 0 : stocktakingOld.SumFact;
            DateTime startDate = stocktakingOld == null ? DateTime.Now.AddDays(-7).Date : stocktakingOld.Start.Date;
            DateTime stopDate = shift.Start.Date;
            decimal cashMoneyOld = stocktakingOld?.CashMoneyFact ?? 0;
            //TODO: Проверить на нужность
            /*
            if (stocktakingOld != null && stocktakingOld.ReportsAfterStocktaking.FirstOrDefault() != null)
                cashMoneyOld = stocktakingOld.ReportsAfterStocktaking.FirstOrDefault().CashStartSum;
            */
            //Приходы
            decimal sumArrival = 0;
            decimal sumArrivalToday = 0;
            var arrivals = await db.Arrivals
                .Where(a => a.isSuccess == true & a.DateArrival.Date >= startDate & a.DateArrival.Date <= stopDate)
                .ToListAsync();
            foreach (var arrival in arrivals)
            {
                sumArrival += arrival.SumArrival;
                sumArrivalToday += arrival.DateArrival.Date == shift.Start.Date ? arrival.SumArrival : 0;
            }
            //Выплаты / Внесения
            var outcomes = await db.CashMoneys
                .Where(c => c.Create.Date >= startDate & c.Create.Date <= stopDate)
                .ToListAsync();
            decimal sumOutcome = outcomes.Where(s=>s.TypeOperation==CashMoneyTypeOperations.Outcome).Sum(s=>s.Sum);
            decimal sumOutcomeToday = outcomes.Where(s => s.TypeOperation == CashMoneyTypeOperations.Outcome & s.Create.Date==stopDate).Sum(s => s.Sum);
            decimal sumIncome = outcomes.Where(s => s.TypeOperation == CashMoneyTypeOperations.Income).Sum(s => s.Sum);
            decimal sumIncomeToday = outcomes.Where(s => s.TypeOperation == CashMoneyTypeOperations.Income & s.Create.Date == stopDate).Sum(s => s.Sum);
            //Терминал
            decimal sumElectron = 0;
            decimal sumDiscount = 0;
            decimal sumElectronToday = shift.SumElectron;
            decimal sumDiscountToday = shift.SumDiscount;
            var shifts = await db.Shifts
                .Where(s => s.Start.Date >= startDate & s.Start.Date <= stopDate)
                .ToListAsync();
            foreach (var shift1 in shifts)
            {
                sumElectron += shift1.SumElectron;
                sumDiscount += shift1.SumDiscount;
            }
            //Списания
            decimal sumWriteof = 0;
            decimal sumWriteOfToday = 0;
            var writeofs = await db.Writeofs
                .Where(w => w.IsSuccess == true & w.DateWriteof.Date >= startDate & w.DateWriteof.Date <= stopDate)
                .ToListAsync();
            foreach (var writeof in writeofs)
            {
                sumWriteof += writeof.SumAll;
                sumWriteOfToday += writeof.DateWriteof.Date == stopDate ? writeof.SumAll : 0;
            }

            //Переоценка
            decimal revaluationWriteOf = 0;
            decimal revaluationArrival = 0; 
            decimal revaluationWriteOfToday = 0;
            decimal revaluationArrivalToday = 0;
            var revaluations = await db.Revaluations
                .Where(r => r.Create >= startDate & r.Create <= stopDate).ToListAsync();
            foreach (var revaluation in revaluations)
            {
                revaluationWriteOf += revaluation.SumOld;
                revaluationWriteOfToday += revaluation.Create == stopDate ? revaluation.SumOld : 0;
                revaluationArrival += revaluation.SumNew;
                revaluationArrivalToday+= revaluation.Create == stopDate ? revaluation.SumNew : 0;
            }
            //Остаток денег в кассе
            var cashMoneyBalance = await db.MoneyBalanceHistories.Where(m => m.DateBalance.Date == stopDate).FirstOrDefaultAsync();
            decimal cashMoneyEnd = cashMoneyBalance == null ? 0 : cashMoneyBalance.SumEnd;

            //Товарар в магазине на сумму
            decimal sumGoodAll = await db.GoodCountBalanceCurrents.Include(b => b.Good).SumAsync(b => b.Count * b.Good.Price);

            decimal docAllSum = stocktakingOldSum + cashMoneyOld + sumArrival + revaluationArrival - sumDiscount - revaluationWriteOf - sumIncome - sumElectron - sumWriteof - cashMoneyEnd;
            await _notificationService.Send($@"Итоговый отчет после закрытия смены № {shift.Id} от {shift.Start.ToString("dd.MM")}:
Иверторизация было - {stocktakingOldSum}
Было денег в кассе - {cashMoneyOld}
Приход - {sumArrival} {(sumArrivalToday!=0 ? $"+ {sumArrivalToday}" : "")}
Выплаты - {sumOutcome} {(sumOutcomeToday != 0 ? $"+ {sumOutcomeToday}" : "")}
Внесение денег - {sumIncome} {(sumIncomeToday != 0 ? $"+ {sumIncomeToday}" : "")}
Терминал - {sumElectron} {(sumElectronToday != 0 ? $"+ {sumElectronToday}" : "")}
Скидки - {sumDiscount} {(sumDiscountToday !=0 ? $"+ {sumDiscountToday}" : "")}
Списание - {sumWriteof} {(sumWriteOfToday != 0 ? $"+ {sumWriteOfToday}" : "")}
Переоценка (минус) - {revaluationWriteOf} {(revaluationWriteOfToday != 0 ? $"+ {revaluationWriteOfToday}" : "")}
Переоценка (плюс) - {revaluationArrival} {(revaluationArrivalToday != 0 ? $"+ {revaluationArrivalToday}" : "")}

Должны быть остатки по документам - {docAllSum}

Фактически товаров на сумму - {sumGoodAll}

Денег в кассе - {cashMoneyEnd}
");
        }

        public async Task<bool> OpenShift(int idShop, Guid uuid, DateTime start)
        {
            try
            {
                var shift = await db.Shifts.Where(s => s.ShopId == idShop & s.Stop==null).FirstOrDefaultAsync();
                if (shift != null)
                    throw new Exception("Нет закрытых смен");
                var cashier = await db.Cashiers.FirstOrDefaultAsync();

                var shop = await db.Shops.Where(s => s.Id == idShop).FirstOrDefaultAsync();
                if (shop == null)
                    throw new Exception("Магазин не найден");
                db.Shifts.Add(new Shift
                {
                    Uuid=uuid,
                    ShopId = idShop,
                    CashierId = cashier.Id,
                    Start = start
                });
                await db.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public Task<bool> Return(int idShop, List<CashBoxBuyReturnModel> buylist)
        {
            throw new NotImplementedException();
        }
    }
}
