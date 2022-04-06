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

        public async Task<bool> CalcAsync(int ShopId, DateTime date)
        {
            date = date.Date;
            for (DateTime curDate = date.Date; curDate <= DateTime.Now.Date; curDate = curDate.AddDays(1).Date)
            {
                var goodBlancesHistoryDb = await db.GoodBalanceHistories.Where(b => DateTime.Compare(b.CurDate, curDate) == 0).ToListAsync();
                foreach (var balance in goodBlancesHistoryDb)
                    db.GoodBalanceHistories.Remove(balance);
                await db.SaveChangesAsync();
                var shop = await db.Shops.Where(s => s.Id == ShopId).FirstOrDefaultAsync();
                Dictionary<int, double> balanceDict = new Dictionary<int, double>();

                var historyLastDay = await db.GoodBalanceHistories
                    .Where(h => h.ShopId == ShopId & DateTime.Compare(h.CurDate, curDate.AddDays(-1)) == 0)
                    .ToListAsync();
                foreach (var history in historyLastDay)
                    BalanceGoodPlus(balanceDict, history.GoodId, (double) history.CountLast);

                var stocktaking = await db.Stocktakings
                    .Include(s=>s.StocktakingSummaryGoods)
                    .Where(s => s.ShopId == ShopId & DateTime.Compare(s.Create, curDate) == 0 & s.Status==DocumentStatus.Confirm)
                    .OrderByDescending(s=>s.Num)
                    .FirstOrDefaultAsync();
                DateTime startStocktaking = curDate;
                if (stocktaking != null)
                {
                    foreach (var summary in stocktaking.StocktakingSummaryGoods)
                        if (balanceDict.ContainsKey(summary.GoodId))
                            balanceDict[summary.GoodId] = (double)summary.CountFact;
                        else
                            balanceDict.Add(summary.GoodId, (double)summary.CountFact);
                    startStocktaking = stocktaking.Start;
                }

                var arrivalLastDay = await db.Arrivals.Include(a => a.ArrivalGoods)
                    .Where(a => a.ShopId == ShopId & a.isSuccess & DateTime.Compare(a.DateArrival, curDate) == 0)
                    .ToListAsync();
                foreach (var arrival in arrivalLastDay)
                    foreach (var arrivalgood in arrival.ArrivalGoods)
                        BalanceGoodPlus(balanceDict, arrivalgood.GoodId, (double)arrivalgood.Count);

                var writeofs = await db.Writeofs.Include(w => w.WriteofGoods)
                    .Where(w => w.ShopId == ShopId & w.IsSuccess & DateTime.Compare(w.DateWriteof, curDate) == 0)
                    .ToListAsync();
                foreach (var writeof in writeofs)
                    foreach (var wgood in writeof.WriteofGoods)
                        BalanceGoodMinus(balanceDict, wgood.GoodId, wgood.Count);

                var shifts = await db.Shifts
                    .Include(s => s.ShiftSales)
                    .Where(s => s.ShopId == ShopId && DateTime.Compare(s.Start, startStocktaking) >= 0 && DateTime.Compare(s.Start.Date,curDate)==0)
                    .ToListAsync();
                foreach (var shift in shifts)
                    foreach (var sale in shift.ShiftSales)
                    {
                        if (sale.Count > 0)
                            BalanceGoodMinus(balanceDict, sale.GoodId, sale.Count);
                        if (sale.CountReturn > 0)
                            BalanceGoodPlus(balanceDict, sale.GoodId, (double)sale.CountReturn);
                    }

                foreach (var balance in balanceDict)
                    db.GoodBalanceHistories.Add(new GoodBalanceHistory
                    {
                        Shop = shop,
                        CurDate = curDate.Date,
                        GoodId = balance.Key,
                        CountLast = (decimal)balance.Value
                    });

                if (date == curDate)
                {
                    var balances = await db.GoodBalances.Where(b => b.ShopId == ShopId).ToListAsync();
                    foreach (var balance in balances)
                        db.GoodBalances.Remove(balance);
                    foreach (var balance in balanceDict)
                        db.GoodBalances.Add(new GoodBalance { ShopId = ShopId, GoodId = balance.Key, Count = balance.Value });
                }

                await db.SaveChangesAsync();
            }

            return true;
        }

        void BalanceGoodPlus(Dictionary<int, double> balance, int goodId, double count)
        {
            if (balance.ContainsKey(goodId))
                balance[goodId] += count;
            else
                balance.Add(goodId, count);
        }

        void BalanceGoodMinus(Dictionary<int, double> balance, int goodId, double count)
        {
            if (balance.ContainsKey(goodId))
                balance[goodId] -= count;
            else
                balance.Add(goodId, -1*count);
        }

        public async Task MinusAsync(int ShopId, int GoodId, double Count)
        {
            var balanceHistory = await db.GoodBalanceHistories
                .Where(b => b.ShopId == ShopId & b.GoodId == GoodId & b.CurDate == DateTime.Now.Date).FirstOrDefaultAsync();
            if (balanceHistory != null)
                balanceHistory.CountLast -= (decimal)Count;
            else
                db.GoodBalanceHistories.Add(new GoodBalanceHistory
                {
                    ShopId = ShopId,
                    GoodId = GoodId,
                    CurDate = DateTime.Now,
                    CountLast = -1 * (decimal)Count
                });

            var balance = await db.GoodBalances.Where(b => b.ShopId == ShopId & b.GoodId == GoodId).FirstOrDefaultAsync();
            if (balance == null)
                db.GoodBalances.Add(new GoodBalance
                {
                    GoodId = GoodId,
                    ShopId = ShopId,
                    Count = -1 * Count
                });
            else
                balance.Count -= Count;
            await db.SaveChangesAsync();
        }

        public async Task MinusAsync(List<GoodBalanceModel> goodBalances)
        {
            foreach(var goodModel in goodBalances)
            {
                var gb = await db.GoodBalances.Where(b => b.GoodId == goodModel.GoodId & b.ShopId == goodModel.ShopId).FirstOrDefaultAsync();
                if (gb == null)
                {
                    var newGoodBalance = new GoodBalance
                    {
                        GoodId = goodModel.GoodId,
                        ShopId = goodModel.ShopId,
                        Count = -1 * goodModel.Count
                    };
                    db.GoodBalances.Add(newGoodBalance);
                }
                else
                    gb.Count -= goodModel.Count;
            }
            await db.SaveChangesAsync();
        }

        public async Task PlusAsync(int ShopId, int GoodId, double Count)
        {
            var balanceHistory = await db.GoodBalanceHistories
                .Where(b => b.ShopId == ShopId & b.GoodId == GoodId & b.CurDate == DateTime.Now.Date).FirstOrDefaultAsync();
            if (balanceHistory != null)
                balanceHistory.CountLast += (decimal)Count;
            else
                db.GoodBalanceHistories.Add(new GoodBalanceHistory
                {
                    ShopId = ShopId,
                    GoodId = GoodId,
                    CurDate = DateTime.Now,
                    CountLast = (decimal)Count
                });

            var balance = await db.GoodBalances.Where(b => b.ShopId == ShopId & b.GoodId == GoodId).FirstOrDefaultAsync();
            if (balance == null)
                db.GoodBalances.Add(new GoodBalance
                {
                    GoodId = GoodId,
                    ShopId = ShopId,
                    Count =  Count
                });
            else
                balance.Count += Count;
            await db.SaveChangesAsync();
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
