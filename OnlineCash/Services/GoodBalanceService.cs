using Microsoft.Extensions.Logging;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineCash.Services
{
    public class GoodBalanceService : IGoodBalanceService
    {
        shopContext db;
        ILogger<GoodBalanceService> logger;
        public GoodBalanceService(shopContext db, ILogger<GoodBalanceService> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public async Task MinusAsync(List<GoodBalanceModel> model)
        {
            throw new NotImplementedException();
        }

        public async Task PlusAsync(List<GoodBalanceModel> model)
        {
            var goodBlanaceDb = await db.GoodBalances.ToListAsync();
            foreach(var goodBalance in GroupingModel(model))
            {
                var balanceDb = goodBlanaceDb.Where(b => b.ShopId == goodBalance.ShopId & b.GoodId == goodBalance.GoodId).FirstOrDefault();
                if (balanceDb != null)
                    balanceDb.Count += goodBalance.Count;
                else
                    db.GoodBalances.Add(new GoodBalance { GoodId = goodBalance.GoodId, ShopId = goodBalance.ShopId, Count = goodBalance.Count });
            };
            await db.SaveChangesAsync();
        }

        public async Task ZerAsynco(Shop shop)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Группировка модели
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        List<GoodBalanceModel> GroupingModel(List<GoodBalanceModel> model)
        {
            var goodBalances = new List<GoodBalanceModel>();
            foreach (var balance in model)
            {
                var b = goodBalances.Where(b => b.GoodId == balance.GoodId & b.ShopId == balance.ShopId).FirstOrDefault();
                if (b != null)
                    b.Count += balance.Count;
                else
                    goodBalances.Add(new GoodBalanceModel { ShopId = balance.ShopId, GoodId = balance.GoodId, Count = balance.Count });
            };
            return goodBalances;
        }
    }
}
