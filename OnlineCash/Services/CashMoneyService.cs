using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Services
{
    public class CashMoneyService
    {
        shopContext _db;
        MoneyBalanceService _moneyBalanceService;
        public CashMoneyService(shopContext db, MoneyBalanceService moneyBalanceService)
        {
            _db = db;
            _moneyBalanceService = moneyBalanceService;
        }

        public async Task<List<CashMoney>> Get() => await _db.CashMoneys.Include(c=>c.Shop).OrderByDescending(c => c.Id).Take(100).ToListAsync();

        public async Task<List<CashMoney>> Get(string? withStr, string? byStr)
        {
            DateTime with = DateTime.Now;
            DateTime.TryParse(withStr, out with);
            DateTime by = DateTime.Now;
            DateTime.TryParse(byStr, out by);

            if (!string.IsNullOrEmpty(withStr) & string.IsNullOrEmpty(byStr))
                return await _db.CashMoneys.Include(c => c.Shop)
                    .Where(c => DateTime.Compare(with, c.Create.Date) <= 0)
                    .ToListAsync();
            if (string.IsNullOrEmpty(withStr) & !string.IsNullOrEmpty(byStr))
                return await _db.CashMoneys.Include(c => c.Shop)
                    .Where(c => DateTime.Compare(c.Create.Date, by) <= 0)
                    .Take(1000)
                    .ToListAsync();
            if (!string.IsNullOrEmpty(withStr) & !string.IsNullOrEmpty(byStr))
                return await _db.CashMoneys.Include(c => c.Shop)
                    .Where(c => DateTime.Compare(c.Create.Date, with) >= 0 & DateTime.Compare(c.Create.Date, by) <= 0)
                    .Take(1000)
                    .ToListAsync();

            return await Get();
        }

        public async Task<CashMoney> Add(int shopId, CashMoney model)
        {
            if (DateTime.Compare(DateTime.Now.Date, model.Create.Date) == -1)
                throw new Exception("В докуменете поступления денег дата болше текущей");
            var shop = _db.Shops.Where(s => s.Id == shopId).FirstOrDefault();
            if (shop == null)
                throw new Exception($"Магазин c id - {shopId} не найден");
            var cashMoney = await _db.CashMoneys.Where(c => c.Uuid == model.Uuid).FirstOrDefaultAsync();
            if (cashMoney != null)
                throw new Exception($"Операция {model.TypeOperation.GetDescription()} с uuid - {model.Uuid} уже существует");
            var cashMoneyNew = new CashMoney { Shop = shop, Uuid = model.Uuid, Create = model.Create, TypeOperation = model.TypeOperation, Sum = model.Sum, Note = model.Note };
            _db.CashMoneys.Add(cashMoneyNew);
            await _db.SaveChangesAsync();
            if (DateTime.Compare(model.Create.Date, DateTime.Now.Date) == -1)
                await _moneyBalanceService.Calculate(shopId, model.Create.Date);
            else
            {
                if (model.TypeOperation == CashMoneyTypeOperations.Income)
                    await _moneyBalanceService.AddIncome(shopId, model.Sum);
                if (model.TypeOperation == CashMoneyTypeOperations.Outcome)
                    await _moneyBalanceService.AddOutcome(shopId, model.Sum);
            }
            return cashMoneyNew;
        }
    }
}
