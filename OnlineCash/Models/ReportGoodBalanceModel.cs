using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Models
{
    public class ReportGoodBalanceModel
    {
        public DateTime CurDate { get; set; }
        public int ShopId { get; set; }
        public int? GoodGroupId { get; set; }
        public List<ReportGoodBalanceGoodModel> Balances { get; set; }
        public double CountAll { get => Math.Round(Balances?.Sum(b => b.Count) ?? 0, 3); }
        public decimal SumAll { get => Math.Round(Balances?.Sum(b => b.Sum) ?? 0,2); }
    }
}
