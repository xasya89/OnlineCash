using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class MoneyBalanceHistory
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public DateTime DateBalance { get; set; }
        public decimal SumStart { get; set; }
        public decimal SumSale { get; set; }
        public decimal SumReturn { get; set; }
        public decimal SumIncome { get; set; }
        public decimal SumOutcome { get; set; }
        public decimal SumOther { get; set; }
        public decimal SumEnd { get; set; }
    }
}
