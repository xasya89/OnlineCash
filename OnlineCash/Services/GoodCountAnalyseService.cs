using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Models;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Services
{
    public class GoodCountAnalyseService
    {
        shopContext _db;
        public GoodCountAnalyseService(shopContext db)
        {
            _db = db;
        }
        public async Task<List<GoodCountAnalyse>> GetAnalyse(int shopId, DateTime start, DateTime stop)
        {
            start = start.Date;
            stop = stop.Date;
            List<GoodCountAnalyse> countAnalyses = new List<GoodCountAnalyse>();
            var goods = await _db.Goods.Where(g => g.IsDeleted == false).ToListAsync();
            foreach(var good in goods)
            {
                var countAnalyse = new GoodCountAnalyse { GoodId=good.Id, GoodName = good.Name };
                for (var day = start; DateTime.Compare( day, stop) <= 0; day = day.AddDays(1).Date)
                    countAnalyse.CountOfDays.Add(day, new GoodCountAnalyseDay());
                countAnalyses.Add(countAnalyse);
            }
            var historyes = await _db.GoodBalanceHistories.Where(h => h.CurDate >= start & h.CurDate <= stop).ToListAsync();
            foreach(var history in historyes)
            {
                var countOfDay = countAnalyses.Where(c => c.GoodId == history.GoodId).FirstOrDefault();
                if (countOfDay != null)
                    countOfDay.CountOfDays[history.CurDate].CountGoodBalance =(decimal) history.CountLast;
            }
            var arrivals = await _db.Arrivals.Include(a => a.ArrivalGoods).Where(a => a.DateArrival >= start & a.DateArrival <= stop).ToListAsync();
            foreach(var arrival in arrivals)
                foreach(var aGood in arrival.ArrivalGoods)
                {
                    var countOfDay = countAnalyses.Where(c => c.GoodId == aGood.GoodId).FirstOrDefault();
                    if (countOfDay != null)
                        countOfDay.CountOfDays[arrival.DateArrival].CountPlus += (decimal)aGood.Count;
                    
                }
            var shifts = await _db.Shifts
                    .Include(s => s.ShiftSales)
                    .Where(s => s.ShopId == shopId && DateTime.Compare(s.Start.Date, start) >= 0 && DateTime.Compare(s.Start.Date, stop) <= 0)
                    .ToListAsync();
            foreach(var shift in shifts)
                foreach(var sGood in shift.ShiftSales)
                {
                    var countOfDay = countAnalyses.Where(c => c.GoodId == sGood.GoodId).FirstOrDefault();
                    if (countOfDay != null)
                        countOfDay.CountOfDays[shift.Start.Date].CountMinus += (decimal)sGood.Count;
                }
            var writeofs = await _db.Writeofs.Include(w => w.WriteofGoods).Where(w => w.IsSuccess & w.ShopId == shopId & DateTime.Compare(w.DateWriteof.Date, start) >= 0 && DateTime.Compare(w.DateWriteof.Date, stop) <= 0).ToListAsync();
            foreach (var writeof in writeofs)
                foreach (var wgood in writeof.WriteofGoods)
                {
                    var countOfDay = countAnalyses.Where(c => c.GoodId == wgood.GoodId).FirstOrDefault();
                    if (countOfDay != null)
                        countOfDay.CountOfDays[writeof.DateWriteof.Date].CountMinus += (decimal)wgood.Count;
                }
            return countAnalyses;
        }
    }
}
