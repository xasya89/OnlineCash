using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Models
{
    public class ReportSellGoodModel
    {
        public Good Good { get; set; }
        public double CountSell { get; set; }
        public decimal CountReturn { get; set; }
        public decimal CountAll { get; set; }
        public decimal SumAll { get; set; }
    }
}
