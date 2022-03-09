using Microsoft.Extensions.Logging;
using OnlineCash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using Microsoft.Extensions.Configuration;

namespace OnlineCash.Services
{
    public class WriteofService : IWriteofService
    {
        shopContext db;
        ILogger<WriteofService> logger;
        IGoodBalanceService goodBalance;
        IConfiguration _configuration;
        public WriteofService(shopContext db, ILogger<WriteofService> logger, IConfiguration configuration, IGoodBalanceService goodBalance)
        {
            this.db = db;
            this.logger = logger;
            this.goodBalance = goodBalance;
            _configuration = configuration;
        }

        public async Task<bool> SaveSynch(int shopId, WriteofSynchModel model)
        {
            bool autoSuccess = _configuration.GetSection("AutoSuccessFromCash").Value == "1";
            var old = await db.Writeofs.Where(w => w.Uuid == model.Uuid & w.ShopId==shopId).FirstOrDefaultAsync();
            if (old != null) return true;
            Writeof writeofDb = new Writeof { 
                IsSuccess=autoSuccess,
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
            if(autoSuccess)
                await goodBalance.CalcAsync(shopId, model.DateCreate);
            return true;
        }
    }
}
