using Microsoft.Extensions.Logging;
using OnlineCash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Services
{
    public class WriteofService : IWriteofService
    {
        shopContext db;
        ILogger<WriteofService> logger;
        IGoodBalanceService goodBalance;
        public WriteofService(shopContext db, ILogger<WriteofService> logger, IGoodBalanceService goodBalance)
        {
            this.db = db;
            this.logger = logger;
            this.goodBalance = goodBalance;
        }

        public async Task<bool> SaveSynch(int shopId, WriteofSynchModel model)
        {
            var old = await db.Writeofs.Where(w => w.Uuid == model.Uuid & w.ShopId==shopId).FirstOrDefaultAsync();
            if (old != null) return true;
            Writeof writeofDb = new Writeof { 
                Uuid = model.Uuid, 
                ShopId=shopId, 
                Status=DocumentStatus.New, 
                DateWriteof = model.DateCreate, 
                Note = model.Note,
                SumAll=model.Goods.Sum(w=>(decimal)w.Count * w.Price)
            };
            db.Writeofs.Add(writeofDb);
            foreach(var wgood in model.Goods)
            {
                Good good = await db.Goods.Where(g => g.Uuid == wgood.Uuid).FirstOrDefaultAsync();
                if (good == null) return false;
                db.WriteofGoods.Add(new WriteofGood
                {
                    Writeof=writeofDb,
                    GoodId = good.Id,
                    Count = wgood.Count,
                    Price = wgood.Price
                });
            };
            await db.SaveChangesAsync();
            //await goodBalance.CalcAsync(shopId, writeofDb.DateWriteof);
            return true;
        }
    }
}
