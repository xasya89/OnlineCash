
using OnlineCash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Services
{
    public class CashBoxService : ICashBoxService
    {
        shopContext db;
        IGoodBalanceService goodBalanceService;
        public CashBoxService(shopContext db, IGoodBalanceService goodBalanceService)
        {
            this.db = db;
            this.goodBalanceService = goodBalanceService;
        }
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
                await goodBalanceService.MinusAsync(shift.ShopId, good.Id, buy.Count);
            }
            shift.SumSell += buylist.Sum(b => (decimal)b.Count * b.Price);
            shift.SumElectron+=buylist.Where(b=>b.IsElectron).Sum(b => (decimal)b.Count * b.Price);
            shift.SumNoElectron+=buylist.Where(b=>b.IsElectron==false).Sum(b => (decimal)b.Count * b.Price);
            shift.SumAll+= buylist.Sum(b => (decimal)b.Count * b.Price);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CloseShift(Guid uuid, DateTime stop)
        {
            var shift = await db.Shifts.Where(s => s.Uuid==uuid & s.Stop == null).FirstOrDefaultAsync();
            if (shift == null)
                return false;
            shift.Stop = stop;
            await goodBalanceService.CalcAsync(shift.ShopId, shift.Start);
            await db.SaveChangesAsync();
            return true;
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
