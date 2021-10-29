using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class ReportGoodBalanceGoodModel
    {
        public string GoodName { get; set; }
        public double Count { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get => Math.Round((decimal)Count * Price,2); }
    }
}
