using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;

namespace OnlineCash.Services
{
    public class ArrivalService
    {
        shopContext db;
        IConfiguration _configuration;
        IGoodBalanceService _goodBalanceService;
        GoodCountBalanceService _countBalanceService;
        public ArrivalService(shopContext db, IConfiguration configuration, IGoodBalanceService goodBalanceService, GoodCountBalanceService countBalanceService)
        {
            this.db = db;
            _configuration = configuration;
            _goodBalanceService = goodBalanceService;
            _countBalanceService = countBalanceService;
        }

        public async Task<Arrival> SaveSynchAsync(int shopId, ArrivalSynchModel model)
        {
            if (db.Shops.Where(s => s.Id == shopId) == null)
                throw new Exception($"Магазин shopId - {shopId} не найден");
            if (db.Suppliers.Where(s => s.Id == model.SupplierId).FirstOrDefault() == null)
                throw new Exception($"Поставщик supplierId - {model.SupplierId} не найден");
            var goods = await db.Goods.Include(g => g.GoodPrices.Where(gp => gp.ShopId == shopId)).ToListAsync();
            foreach (var mGood in model.ArrivalGoods)
                if (goods.Where(g => g.Uuid == mGood.GoodUuid).FirstOrDefault() == null)
                    throw new Exception($"Товар uuid {mGood.GoodUuid} не найден");

            bool autoSuccess = _configuration.GetSection("AutoSuccessFromCash").Value == "1";
            var arrival = new Arrival
            {
                Num = model.Num,
                DateArrival = model.DateArrival,
                ShopId = shopId,
                SupplierId = model.SupplierId,
                SumSell=0,
                isSuccess=autoSuccess
            };
            db.Arrivals.Add(arrival);
            List<ArrivalGood> arrivalGoods = new List<ArrivalGood>();
            foreach(var mGood in model.ArrivalGoods)
            {
                var good = goods.Where(g => g.Uuid == mGood.GoodUuid).FirstOrDefault();
                arrivalGoods.Add(new ArrivalGood
                {
                    Arrival = arrival,
                    GoodId = good.Id,
                    Price = mGood.Price,
                    PriceSell = good.GoodPrices.FirstOrDefault().Price,
                    Count = (double)mGood.Count,
                    Nds = mGood.Nds,
                    ExpiresDate = mGood.ExpiresDate
                });
            }
            db.ArrivalGoods.AddRange(arrivalGoods);
            arrival.SumNds = arrival.ArrivalGoods.Sum(a => a.SumNds);
            arrival.SumArrival = arrival.ArrivalGoods.Sum(a => a.Sum);
            arrival.SumSell = arrival.ArrivalGoods.Sum(a => a.SumSell);

            await db.SaveChangesAsync();
            if(autoSuccess)
                await _countBalanceService.Add<ArrivalGood>(arrival.Id, DateTime.Now, arrivalGoods);

            return arrival;
        }
    }
}
