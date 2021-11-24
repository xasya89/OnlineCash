using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;

namespace OnlineCash.Services
{
    public class ArrivalService
    {
        shopContext db;
        public ArrivalService(shopContext db)
        {
            this.db = db;
        }

        public async Task<bool> SaveSynchAsync(int shopId, ArrivalSynchModel model)
        {
            if (db.Shops.Where(s => s.Id == shopId) == null)
                return false;
            if (db.Suppliers.Where(s => s.Id == model.SupplierId).FirstOrDefault() == null)
                return false;
            var goods = await db.Goods.Include(g => g.GoodPrices.Where(gp => gp.ShopId == shopId)).ToListAsync();
            foreach (var mGood in model.ArrivalGoods)
                if (goods.Where(g => g.Uuid == mGood.GoodUuid).FirstOrDefault() == null)
                    return false;


            var arrival = new Arrival
            {
                Num = model.Num,
                DateArrival = model.DateArrival,
                ShopId = shopId,
                SupplierId = model.SupplierId,
                CountAll = (double)model.ArrivalGoods.Sum(a => a.Count),
                PriceAll = model.ArrivalGoods.Sum(a => a.Count * a.Price),
                SumArrivals= model.ArrivalGoods.Sum(a => a.Count * a.Price)
            };
            db.Arrivals.Add(arrival);
            foreach(var mGood in model.ArrivalGoods)
            {
                var good = goods.Where(g => g.Uuid == mGood.GoodUuid).FirstOrDefault();
                var arrivalgood = new ArrivalGood
                {
                    Arrival = arrival,
                    GoodId = good.Id,
                    Price = mGood.Price,
                    PriceSell = good.GoodPrices.FirstOrDefault().Price,
                    Count = (double) mGood.Count
                };
                db.ArrivalGoods.Add(arrivalgood);
            }
            await db.SaveChangesAsync();
            return true;
        }
    }
}
