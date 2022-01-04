using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Models
{
    public class ReportSellModel
    {
        public Shop Shop { get; set; }
        public DateTime Start { get; set; }
        public decimal CountAll { get; set; }
        public decimal SumSell { get; set; }
        public List<ReportSellGoodModel> ReportGoods { get; set; } = new List<ReportSellGoodModel>();
    }
}
