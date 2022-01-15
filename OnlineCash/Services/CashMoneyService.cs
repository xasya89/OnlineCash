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
        MoneyBalanceService _moneyService;
        public CashMoneyService(shopContext db, MoneyBalanceService moneyService)
        {
            _db = db;
            _moneyService = moneyService;
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

        public async Task Add(int shopId, CashMoney model)
        {
            var shop = _db.Shops.Where(s => s.Id == shopId).FirstOrDefault();
            if (shop == null)
                throw new Exception($"Магазин c id - {shopId} не найден");
            var cashMoney = await _db.CashMoneys.Where(c => c.Uuid == model.Uuid).FirstOrDefaultAsync();
            if (cashMoney != null)
                throw new Exception($"Операция {model.TypeOperation.GetDescription()} с uuid - {model.Uuid} уже существует");
            _db.CashMoneys.Add(new CashMoney { Shop=shop, Uuid = model.Uuid, Create = model.Create, TypeOperation = model.TypeOperation, Sum = model.Sum, Note = model.Note });
            await _db.SaveChangesAsync();
            if (model.TypeOperation == CashMoneyTypeOperations.Income)
                await _moneyService.AddIncome(shopId, model.Sum);
            if (model.TypeOperation == CashMoneyTypeOperations.Outcome)
                await _moneyService.AddOutcome(shopId, model.Sum);
        }
    }
}
