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
        GoodCountBalanceService _countBalanceService;
        public WriteofService(shopContext db, 
            ILogger<WriteofService> logger, 
            IConfiguration configuration, 
            IGoodBalanceService goodBalance,
            GoodCountBalanceService countBalanceService)
        {
            this.db = db;
            this.logger = logger;
            this.goodBalance = goodBalance;
            _configuration = configuration;
            _countBalanceService = countBalanceService;
        }

        public async Task<Writeof> SaveSynch(int shopId, WriteofSynchModel model)
        {
            bool autoSuccess = _configuration.GetSection("AutoSuccessFromCash").Value == "1";
            var old = await db.Writeofs.Where(w => w.Uuid == model.Uuid & w.ShopId==shopId).FirstOrDefaultAsync();
            if (old != null) return null;
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
            List<WriteofGood> writeofGoods = new List<WriteofGood>();
            foreach(var wgood in model.Goods)
            {
                Good good = await db.Goods.Where(g => g.Uuid == wgood.Uuid).FirstOrDefaultAsync();
                if (good == null) return null;
                writeofGoods.Add(new WriteofGood
                {
                    Writeof = writeofDb,
                    GoodId = good.Id,
                    Count = wgood.Count,
                    Price = wgood.Price
                });
            };
            db.WriteofGoods.AddRange(writeofGoods);
            await db.SaveChangesAsync();
            if (autoSuccess)
                await _countBalanceService.Add<WriteofGood>(writeofDb.Id, DateTime.Now, writeofGoods);
                //await goodBalance.CalcAsync(shopId, model.DateCreate);
            return writeofDb;
        }
    }
}
