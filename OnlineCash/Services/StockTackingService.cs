﻿using OnlineCash.Models.StockTackingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Models;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Services
{
    public class StockTackingService : IStockTackingService
    {
        shopContext db;
        public StockTackingService(shopContext db)
        {
            this.db = db;
        }

        public async Task<StockTackingGruppingModel> GetDetailsGroups(int idStocktaking)
        {
            var stocktacking = await db.Stocktakings.Include(st => st.StockTakingGroups)
                .ThenInclude(gr => gr.StocktakingGoods)
                .ThenInclude(g => g.Good)
                .Where(st => st.Id == idStocktaking).FirstOrDefaultAsync();
            var shop = await db.Shops.Where(s => s.Id == stocktacking.ShopId).FirstOrDefaultAsync();
            StockTackingGruppingModel model = new StockTackingGruppingModel {NumDoc=stocktacking.Num, DateDoc=stocktacking.Create, ShopName=shop.Name };
            var goods = await db.Goods.ToListAsync();
            foreach(var group in stocktacking.StockTakingGroups)
                foreach(var good in group.StocktakingGoods)
                {
                    var groupModel = model.Groups.Where(gr => gr.GroupId == good.Good.GoodGroupId).FirstOrDefault();
                    if(groupModel==null)
                    {
                        var gr = db.GoodGroups.Where(gr => gr.Id == good.Good.GoodGroupId).FirstOrDefault();
                        groupModel = new StockTackingGruppingGroupModel { GroupId = gr.Id, GroupName = gr.Name };
                        model.Groups.Add(groupModel);
                    };
                    var goodModel = groupModel.Goods.Where(g => g.GoodId == good.GoodId).FirstOrDefault();
                    if (goodModel == null)
                    {
                        goodModel = new StockTackingGruppingGoodModel { 
                            GoodId = good.GoodId, 
                            GoodName = good.Good.Name, 
                            CountDb = Math.Round(good.CountDB, 3), 
                            CountFact = Math.Round(good.CountFact, 3), 
                            Price = good.Price 
                        };
                        groupModel.Goods.Add(goodModel);
                    }
                    else
                    {
                        goodModel.CountDb += Math.Round(good.CountDB,3);
                        goodModel.CountFact += Math.Round(good.CountFact,3);
                        goodModel.Price = good.Price;
                    }
                }
            return model;
        }

        public async Task SaveFromOnlinCash(int shopId, StocktakingReciveDataModel model)
        {
            var goods = await db.Goods.Include(g=>g.GoodPrices).ToListAsync();
            foreach (var group in model.Groups)
                foreach (var good in group.Goods)
                    if (goods.Where(g => g.Uuid == good.Uuid).FirstOrDefault() == null)
                        throw new Exception($"Товар с uuid - {good.Uuid} не существует");

            var stocktaking = new Stocktaking { Create = model.Create, Num = await db.Stocktakings.MaxAsync(s => s.Num) + 1, ShopId=shopId};
            db.Stocktakings.Add(stocktaking);
            foreach(var group in model.Groups)
            {
                var groupDb = new StockTakingGroup { Stocktaking = stocktaking, Name = group.Name };
                db.StockTakingGroups.Add(groupDb);
                foreach(var good in group.Goods)
                {
                    var gooddb = goods.Where(g => g.Uuid == good.Uuid).FirstOrDefault();
                    var pricedb = gooddb.GoodPrices.Where(p => p.ShopId == shopId).FirstOrDefault().Price;
                    db.StocktakingGoods.Add(new StocktakingGood { StockTakingGroup=groupDb, GoodId = gooddb.Id, Price = pricedb, CountFact = good.CountFact });
                }
            }
            await db.SaveChangesAsync();
        }
    }

    public class StocktakingReciveDataModel
    {
        public DateTime Create { get; set; }
        public List<StocktakingGroupReciveDataModel> Groups { get; set; } = new List<StocktakingGroupReciveDataModel>();
    }

    public class StocktakingGroupReciveDataModel
    {
        public string Name { get; set; }
        public List<StocktakingGoodReciveDataModel> Goods { get; set; } = new List<StocktakingGoodReciveDataModel>();
    }
    public class StocktakingGoodReciveDataModel
    {
        public Guid Uuid { get; set; }
        public double CountFact { get; set; }
    }
}
