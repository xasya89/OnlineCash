using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class CashBoxBuyReturnModel
    {
        public bool IsElectron { get; set; }
        public Guid Uuid { get; set; }
        public double Count { get; set; }
        public decimal Price { get; set; }
    }
}
