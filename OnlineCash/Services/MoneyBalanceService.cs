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
            var moneyBalance = await _db.MoneyBalanceHistories.Where(m => m.ShopId == shopId & DateTime.Compare(m.DateBalance, DateTime.Now.Date) == 0).FirstOrDefaultAsync();
            if (moneyBalance == null)
            {
                decimal sumStart = 0;
                var balanceStart = await _db.MoneyBalanceHistories.Where(m => m.ShopId == shopId & DateTime.Compare(m.DateBalance, DateTime.Now.Date.AddDays(-1)) == 0).FirstOrDefaultAsync();
                if (balanceStart != null)
                    sumStart = balanceStart.SumEnd;
                moneyBalance = new MoneyBalanceHistory { ShopId = shopId, DateBalance = DateTime.Now.Date, SumStart=sumStart };
                _db.MoneyBalanceHistories.Add(moneyBalance);
                await _db.SaveChangesAsync();
            }
            return moneyBalance;
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
