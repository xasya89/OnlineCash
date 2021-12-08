using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models.Discounts
{
    public class DiscountParamSumOneBuyModel:DiscountParamModel
    {
        public decimal SumBuy { get; set; }
        public decimal? DiscountSum { get; set; }
        public int? DiscountPercent { get; set; }
    }
}
