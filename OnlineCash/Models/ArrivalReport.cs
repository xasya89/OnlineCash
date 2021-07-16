using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Models
{
    public class ArrivalReport
    {
        public Supplier Supplier { get; set; }
        public decimal SumPayments { get; set; }
        public decimal SumArrivals { get; set; }
    }
}
