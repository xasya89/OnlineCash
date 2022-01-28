using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Services
{
    public class MoneyBalanceService
    {
        shopContext _db;
        ILogger<MoneyBalanceService> _logger;
        public MoneyBalanceService(shopContext db, ILogger<MoneyBalanceService> logger)
        {
            _db = db;
            _logger = logger;
        }

        private async Task<MoneyBalanceHistory> GetToday(int shopId)
        {
            var shop = _db.Shops.Where(s => s.Id == shopId).FirstOrDefault();
            if (shop == null)
                throw new Exception("Магазин не найден");
            var balanceNowAndLast = await _db.MoneyBalanceHistories.Where(m => m.ShopId == shopId & DateTime.Compare(m.DateBalance, DateTime.Now.Date) <= 0).OrderByDescending(m=>m.DateBalance).Take(2).ToListAsync();
            var balanceNow = balanceNowAndLast.Where(b => DateTime.Compare(b.DateBalance, DateTime.Now.Date) == 0).FirstOrDefault();
            if (balanceNow== null)
            {
                var last = balanceNowAndLast.FirstOrDefault();
                decimal sumStart = 0;
                if(last!=null)
                {
                    last.SumEnd= last.SumStart + last.SumSale + last.SumIncome - last.SumReturn - last.SumOutcome + last.SumOther;
                    sumStart = last.SumEnd;
                }
                balanceNow = new MoneyBalanceHistory { ShopId = shopId, DateBalance = DateTime.Now.Date, SumStart = sumStart, SumEnd=sumStart };
                _db.MoneyBalanceHistories.Add(balanceNow);
                await _db.SaveChangesAsync();
            }
            return balanceNow;
        }

        public async Task Calculate(int shopId,DateTime with)
        {
            with = with.Date;
            for(DateTime curDay=with; DateTime.Compare(curDay, DateTime.Now.Date)<=0; curDay = curDay.AddDays(1).Date)
            {
                var last = await _db.MoneyBalanceHistories.Where(b => DateTime.Compare(b.DateBalance, curDay) < 0).OrderBy(b=>b.Id).LastOrDefaultAsync();
                decimal sumStart = 0;
                if (last != null)
                    sumStart = last.SumEnd;
                var curBalance = await _db.MoneyBalanceHistories.Where(b => DateTime.Compare(b.DateBalance, curDay) == 0).FirstOrDefaultAsync();
                if (curBalance != null)
                    _db.MoneyBalanceHistories.Remove(curBalance);
                var curBalanceNew = new MoneyBalanceHistory { DateBalance = curDay, ShopId = shopId, SumStart = sumStart, SumEnd = sumStart };
                foreach(var money in await _db.CashMoneys.Where(m=>DateTime.Compare(m.Create.Date, curDay) ==0).ToListAsync())
                    switch (money.TypeOperation)
                    {
                        case CashMoneyTypeOperations.Income:
                            curBalanceNew.SumIncome += money.Sum;
                            break;
                        case CashMoneyTypeOperations.Outcome:
                            curBalanceNew.SumOutcome += money.Sum;
                            break;
                    };
                foreach(var shift in await _db.Shifts.Where(s=>s.Start.Date== curDay).ToListAsync())
                {
                    curBalanceNew.SumSale += shift.SumNoElectron;
                    curBalanceNew.SumReturn += shift.SumReturnCash;
                }
                curBalanceNew.SumEnd = sumStart + curBalanceNew.SumSale + curBalanceNew.SumIncome - curBalanceNew.SumReturn - curBalanceNew.SumOutcome + curBalanceNew.SumOther;
                _db.MoneyBalanceHistories.Add(curBalanceNew);
                await _db.SaveChangesAsync();
            }
        }

        public async Task AddSale(int shopId, decimal sum)
        {
            try
            {
                var balance = await GetToday(shopId);
                balance.SumSale += sum;
                balance.SumEnd += sum;
                await _db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Ошибка добавления в MoneyBalance продажи на сумму ${sum} ShopId - {shopId} \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        public async Task AddReturn(int shopId, decimal sum)
        {
            try
            {
                var balance = await GetToday(shopId);
                balance.SumReturn += sum;
                balance.SumEnd -= sum;
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка добавления в MoneyBalance возврата на сумму ${sum} ShopId - {shopId} \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        public async Task AddIncome(int shopId, decimal sum)
        {
            try
            {
                var balance = await GetToday(shopId);
                balance.SumIncome += sum;
                balance.SumEnd += sum;
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка добавления в MoneyBalance внесения денег на сумму ${sum} ShopId - {shopId} \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        public async Task AddOutcome(int shopId, decimal sum)
        {
            try
            {
                var balance = await GetToday(shopId);
                balance.SumOutcome += sum;
                balance.SumEnd -= sum;
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка добавления в MoneyBalance изъятия денег на сумму ${sum} ShopId - {shopId} \n {ex.Message} \n {ex.StackTrace}");
            }
        }
    }
}
