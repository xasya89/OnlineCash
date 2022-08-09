using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Stop { get; set; }
        public decimal SumNoElectron { get; set; } = 0;
        public decimal SumElectron { get; set; } = 0;
        public decimal SumSell { get; set; } = 0;
        public decimal SummReturn { get; set; } = 0;
        public decimal SumIncome { get; set; } = 0;
        public decimal SumOutcome { get; set; } = 0;
        public decimal SumCredit { get; set; } = 0;
        public decimal SumAll { get; set; } = 0;
        public int ShopId { get; set; }
        public int CashierId { get; set; }
        public bool isSynch { get; set; } = false;
        public List<CheckSell> CheckSells { get; set; }
        public List<Credit> Credits { get; set; }
    }
}
