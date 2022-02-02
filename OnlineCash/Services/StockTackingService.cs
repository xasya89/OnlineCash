﻿using OnlineCash.Models.StockTackingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Models;
using OnlineCash.DataBaseModels;
using OnlineCash.Services;

namespace OnlineCash.Services
{
    public class StockTackingService : IStockTackingService
    {
        shopContext db;
        IGoodBalanceService _goodBalanceService;
        public StockTackingService(shopContext db, IGoodBalanceService goodBalanceService)
        {
            this.db = db;
            _goodBalanceService = goodBalanceService;
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

        public async Task<Stocktaking> GetSummary(int stocktakingId)
        {
            var stocktaking= await db.Stocktakings.Include(s => s.StocktakingSummaryGoods).ThenInclude(g => g.Good).Where(s => s.Id == stocktakingId).FirstOrDefaultAsync();
            if (stocktaking == null)
                stocktaking = new Stocktaking();
            return stocktaking;
        }

        public async Task SaveFromOnlinCash(int shopId, StocktakingReciveDataModel model)
        {
            var goods = await db.Goods.Include(g => g.GoodPrices).ToListAsync();
            foreach (var group in model.Groups)
                foreach (var good in group.Goods)
                    if (goods.Where(g => g.Uuid == good.Uuid).FirstOrDefault() == null)
                        throw new Exception($"Товар с uuid - {good.Uuid} не существует");

            var stocktaking = new Stocktaking {
                Create = model.Create,
                Start = model.Create,
                Num = await db.Stocktakings.MaxAsync(s => s.Num) + 1,
                ShopId = shopId,
                Status = DocumentStatus.Confirm,
                isSuccess = true
            };
            db.Stocktakings.Add(stocktaking);
            foreach (var group in model.Groups)
            {
                var groupDb = new StockTakingGroup { Stocktaking = stocktaking, Name = group.Name };
                db.StockTakingGroups.Add(groupDb);
                foreach (var good in group.Goods)
                {
                    var gooddb = goods.Where(g => g.Uuid == good.Uuid).FirstOrDefault();
                    var pricedb = gooddb.GoodPrices.Where(p => p.ShopId == shopId).FirstOrDefault().Price;
                    db.StocktakingGoods.Add(new StocktakingGood { StockTakingGroup = groupDb, GoodId = gooddb.Id, Price = pricedb, CountFact = good.CountFact });
                }
            }
            
            await db.SaveChangesAsync();
            await SuccessCalc(stocktaking.Id);
        }

        public async Task Save(StockTakingSaveModel model)
        {
            Stocktaking stocktaking = await db.Stocktakings.Include(s => s.StockTakingGroups).ThenInclude(gr => gr.StocktakingGoods).Where(s => s.Id == model.id).FirstOrDefaultAsync();
            bool isSuccessOld = stocktaking.isSuccess;
            stocktaking.isSuccess = model.isSuccess;
            stocktaking.Status = model.isSuccess ? DocumentStatus.Confirm : stocktaking.Status;
            foreach (var sgr in model.Groups)
            {
                StockTakingGroup group = null;
                if (sgr.Id == -1)
                {
                    group = new StockTakingGroup
                    {
                        Name = sgr.Name,
                        Stocktaking = stocktaking
                    };
                    db.StockTakingGroups.Add(group);
                }
                else
                {
                    group = await db.StockTakingGroups.Where(gr => gr.Id == sgr.Id).FirstOrDefaultAsync();
                    if (group == null)
                        throw new Exception("Не найден идентификатор группы");
                    group.Name = sgr.Name;
                };
                //Добавим новые товары
                foreach (var sg in sgr.Goods.Where(g => g.id == -1).ToList())
                {
                    var good = await db.Goods.Include(g => g.GoodPrices).Where(g => g.Id == sg.idGood).FirstOrDefaultAsync();
                    StocktakingGood stocktakingGood = new StocktakingGood
                    {
                        StockTakingGroup = group,
                        Good = good,
                        Count = 0,
                        CountDB = 0,
                        CountFact = sg.CountFact,
                        Price = good.GoodPrices.Where(p => p.ShopId == stocktaking.ShopId).FirstOrDefault().Price
                    };
                    db.StocktakingGoods.Add(stocktakingGood);
                }
                //Изменим количество
                foreach (var sg in sgr.Goods.Where(g => g.id != -1).ToList())
                {
                    StocktakingGood stocktakingGood = await db.StocktakingGoods.Where(g => g.Id == sg.id).FirstOrDefaultAsync();
                    stocktakingGood.CountFact = sg.CountFact;
                }
            }
            await db.SaveChangesAsync();
            if (model.isSuccess & !isSuccessOld)
                await SuccessCalc(stocktaking.Id);
        }

        private async Task SuccessCalc(int stocktakingId)
        {
            var stocktaking = await db.Stocktakings.Include(s=>s.StockTakingGroups).ThenInclude(gr=>gr.StocktakingGoods).Where(s => s.Id == stocktakingId).FirstOrDefaultAsync();
            List<StocktakingSummaryGood> summaryes = new List<StocktakingSummaryGood>();
            var balances = await db.GoodBalances.ToListAsync();
            foreach (var group in stocktaking.StockTakingGroups)
                foreach (var good in group.StocktakingGoods)
                {
                    var summary = summaryes.Where(s => s.GoodId == good.GoodId).FirstOrDefault();
                    decimal countDb = 0;
                    var goodBalance = balances.Where(b => b.GoodId == good.GoodId).FirstOrDefault();
                    if (goodBalance != null)
                        countDb = (decimal)goodBalance.Count;
                    if (summary == null)
                        summaryes.Add(new StocktakingSummaryGood
                        {
                            Stocktaking = stocktaking,
                            GoodId = good.GoodId,
                            CountFact = (decimal)good.CountFact,
                            CountDb = countDb,
                            Price = good.Price
                        });
                    else
                    {
                        summary.CountDb += (decimal)countDb;
                        summary.CountFact += (decimal)good.CountFact;
                    }
                }
            //Найдем не указанные в инверторизации товары и добавим их в нераспределенные
            var goodNotAdded = new List<StocktakingGood>();

            
            var goods = await db.Goods.Include(g=>g.GoodPrices.Where(s=>s.ShopId==stocktaking.ShopId)).Where(g => g.IsDeleted == false).ToListAsync();
            var groupNoFind = new StockTakingGroup { Stocktaking = stocktaking, Name = "Не указан в инверторизации" };
            foreach (var balance in balances)
                if (balance.Count != 0)
                {
                    var find = false;
                    var good = goods.Where(g => g.Id == balance.GoodId).FirstOrDefault();
                    var pricedb = good?.GoodPrices.FirstOrDefault().Price;
                    if (good != null)
                    {
                        foreach (var stGroup in stocktaking.StockTakingGroups)
                            foreach (var stGood in stGroup.StocktakingGoods)
                                if (stGood.GoodId == balance.Good.Id)
                                    find = true;
                        if (!find)
                        {
                            goodNotAdded.Add(new StocktakingGood { StockTakingGroup = groupNoFind, GoodId = good.Id, CountDB = balance.Count, CountFact = 0 });
                            summaryes.Add(new StocktakingSummaryGood
                            {
                                Stocktaking = stocktaking,
                                GoodId = good.Id,
                                Price = (decimal)pricedb,
                                CountDb = (decimal)balance.Count,
                                CountFact = 0
                            });
                        }
                    }
                };
            if(goodNotAdded.Count>0)
            {
                db.StockTakingGroups.Add(groupNoFind);
                db.StocktakingGoods.AddRange(goodNotAdded);
            };
            db.StocktakingSummaryGoods.AddRange(summaryes);

            //запишем итоги
            stocktaking.SumDb = summaryes.Sum(s => s.Price * s.CountDb);
            stocktaking.SumFact = summaryes.Sum(s => s.Price * s.CountFact);

            await db.SaveChangesAsync();
            await _goodBalanceService.CalcAsync(stocktaking.ShopId, stocktaking.Start);
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
