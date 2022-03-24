using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Services
{
    public class GoodCountBalanceService
    {
        shopContext _db;
        public GoodCountBalanceService(shopContext db)
        {
            _db = db;
        }

        public async Task AddPlus(DateTime date, TypeDocs typeDoc, int docId, Good good, decimal count)
        {
            GoodCountDocHistory history = new GoodCountDocHistory
            {
                Date = date,
                TypeDoc = typeDoc,
                DocId = docId,
                Good = good,
                Count = count
            };
            _db.GoodCountDocHistories.Add(history);

            await _db.SaveChangesAsync();
        }

        public async Task Init()
        {
            DateTime periodOld = Convert.ToDateTime($"01.{DateTime.Now.Month}.{DateTime.Now.Year}").Date;
            DateTime periodNew = Convert.ToDateTime($"01.{DateTime.Now.AddMonths(1).Month}.{DateTime.Now.AddMonths(1).Year}").Date;

            var goods = await _db.Goods.ToListAsync();
            var oldCurrents = new List<GoodCountBalance>();
            var newCurrents = new List<GoodCountBalance>();
            var balances = await _db.GoodBalances.ToListAsync();
            foreach (var good in goods)
            {
                oldCurrents.Add(new GoodCountBalance { Period=periodOld, GoodId = good.Id, Count = 0 });
                var balance = balances.Where(b => b.GoodId == good.Id).FirstOrDefault();
                newCurrents.Add(new GoodCountBalance { Period=periodNew, GoodId = good.Id, Count = (decimal) (balance?.Count ?? 0)  });
                _db.GoodCountBalanceCurrents.Add(new GoodCountBalanceCurrent { GoodId = good.Id, Count = (decimal)(balance?.Count ?? 0) });
            };

            var arrivals = await _db.Arrivals.Include(a => a.ArrivalGoods).Where(a => a.DateArrival > periodOld).ToListAsync();
            foreach(var arrival in arrivals)
                foreach(var agood in arrival.ArrivalGoods)
                {
                    _db.GoodCountDocHistories.Add(new GoodCountDocHistory
                    {
                        TypeDoc = TypeDocs.Arrival,
                        DocId = arrival.Id,
                        Date = arrival.DateArrival,
                        GoodId = agood.GoodId,
                        Count = (decimal) agood.Count
                    });
                }
            var writeofs = await _db.Writeofs.Include(w=>w.WriteofGoods).Where(w => w.DateWriteof > periodOld).ToListAsync();
            foreach(var writeof in writeofs)
                foreach(var wGood in writeof.WriteofGoods)
                    _db.GoodCountDocHistories.Add(new GoodCountDocHistory
                    {
                        TypeDoc = TypeDocs.WriteOf,
                        DocId = writeof.Id,
                        Date = writeof.DateWriteof,
                        GoodId = wGood.GoodId,
                        Count = (decimal)wGood.Count
                    });
            var shifts = await _db.Shifts.Include(s=>s.ShiftSales).Where(s => s.Start > periodOld).ToListAsync();
            foreach(var shift in shifts)
                foreach(var sgood in shift.ShiftSales)
                    _db.GoodCountDocHistories.Add(new GoodCountDocHistory
                    {
                        TypeDoc = TypeDocs.Shift,
                        DocId = shift.Id,
                        Date = shift.Start,
                        GoodId = sgood.GoodId,
                        Count = (decimal)sgood.Count
                    });
            await _db.SaveChangesAsync();
        }

        public async Task NewGood(int goodId)
        {
            DateTime minPeriod= await _db.GoodCountBalances.MinAsync(b => b.Period);
            DateTime maxPeriod = await _db.GoodCountBalances.MaxAsync(b => b.Period);
            for (DateTime i = minPeriod; i <= maxPeriod; i = i.AddDays(1))
                _db.GoodCountBalances.Add(new GoodCountBalance { Period = i, GoodId = goodId, Count = 0 });
            _db.GoodCountBalanceCurrents.Add(new GoodCountBalanceCurrent { GoodId = goodId, Count=0 });
            await _db.SaveChangesAsync();
        }

        public async Task Add<T>(int docId, DateTime docDate, IEnumerable<T> document)
        {
            TypeDocs typeDoc=TypeDocs.Arrival;
            List<GoodCount> goodCounts = new List<GoodCount>();
            var typeDocument = document.GetType();
            if (typeDocument == typeof(IEnumerable<ArrivalGood>) || typeDocument==typeof(List<ArrivalGood>))
            {
                typeDoc = TypeDocs.Arrival;
                var arrivalGoods = (IEnumerable<ArrivalGood>)document;
                foreach (var arrivalGood in arrivalGoods)
                    goodCounts.Add(new GoodCount { GoodId = arrivalGood.GoodId, Count = (decimal)arrivalGood.Count });
            };
            if (typeDocument == typeof(IEnumerable<WriteofGood>) || typeDocument==typeof(List<WriteofGood>))
            {
                typeDoc = TypeDocs.WriteOf;
                var writeofGoods = (IEnumerable<WriteofGood>)document;
                foreach (var writeofGood in writeofGoods)
                    goodCounts.Add(new GoodCount { GoodId = writeofGood.GoodId, Count = -1M * (decimal)writeofGood.Count });
            };
            foreach (var goodCount in goodCounts)
            {
                _db.GoodCountDocHistories.Add(new GoodCountDocHistory
                {
                    DocId = docId,
                    Date = docDate,
                    TypeDoc = typeDoc,
                    GoodId = goodCount.GoodId,
                    Count = goodCount.Count
                });
            }
            await _db.SaveChangesAsync();
            foreach (var goodCount in goodCounts)
                await AddGoodCountBalance(docDate, goodCount.GoodId, goodCount.Count);
        }

        public async Task AddSell(int shiftId, DateTime date, int goodId, decimal count)
        {
            var balance = await _db.GoodCountDocHistories.Where(h => h.DocId == shiftId).FirstOrDefaultAsync();
            if (balance == null)
                _db.GoodCountDocHistories.Add(new GoodCountDocHistory { DocId = shiftId, Date = date, TypeDoc = TypeDocs.Shift, GoodId = goodId, Count = -1M * count });
            else
                balance.Count -= count;
            await _db.SaveChangesAsync();
            await AddGoodCountBalance(date, goodId, -1M * count);
        }

        public async Task AddReturn(int shiftId, DateTime date, int goodId, decimal count)
        {
            var balance = await _db.GoodCountDocHistories.Where(h => h.DocId == shiftId & h.GoodId==goodId).FirstOrDefaultAsync();
            if (balance == null)
                _db.GoodCountDocHistories.Add(new GoodCountDocHistory { DocId = shiftId, Date = date, TypeDoc = TypeDocs.Shift, GoodId = goodId, Count = count });
            else
                balance.Count += count;
            await _db.SaveChangesAsync();
            await AddGoodCountBalance(date, goodId, count);
        }

        private async Task AddGoodCountBalance(DateTime date, int goodId, decimal count)
        {
            date = date.AddMonths(1);
            date = Convert.ToDateTime($"01.{date.Month}.{date.Year}").Date;
            var balance= await _db.GoodCountBalances.Where(b => b.GoodId == goodId & DateTime.Compare(b.Period, date) == 0).FirstOrDefaultAsync();
            if (balance != null)
                balance.Count += count;
            var current = await _db.GoodCountBalanceCurrents.Where(c => c.GoodId == goodId).FirstOrDefaultAsync();
            current.Count += count;
            await _db.SaveChangesAsync();
        }

        private class GoodCount
        {
            public int GoodId { get; set; }
            public decimal Count { get; set; }
        }
    }
}
