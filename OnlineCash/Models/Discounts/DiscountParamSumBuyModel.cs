using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models.Discounts
{
    public class DiscountParamSumBuyModel:DiscountParamModel
    {
        public decimal SumBuyesMore { get; set; }
        public int DiscountPercent { get; set; }
    }
}
