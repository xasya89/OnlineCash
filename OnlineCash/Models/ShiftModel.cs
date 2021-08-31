using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class ShiftModel
    {
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public decimal SumNoElectron { get; set; } = 0;
        public decimal SumElectron { get; set; } = 0;
        public decimal SumSell { get; set; }
        public decimal SummReturn { get; set; }
        public decimal SumIncome { get; set; }
        public decimal SumOutcome { get; set; }
        public decimal SumAll { get; set; }
        public int ShopId { get; set; }
        public List<CheckSellModel> CheckSells { get; set; }
    }
}
