using OnlineCash.Models.StockTackingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
    }
}
