using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class Shift
    {
        public int Id { get; set; }
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime? Stop { get; set; }
        public decimal SumAll { get; set; } = 0;
        public decimal SumSell { get; set; } = 0;
        public decimal SummReturn { get; set; } = 0;
        public decimal SumIncome { get; set; } = 0;
        public decimal SumOutcome { get; set; } = 0;
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public int CashierId { get; set; }
        public Cashier Cashier { get; set; }
        public List<CheckSell> CheckSells { get; set; } = new List<CheckSell>();
    }
}
