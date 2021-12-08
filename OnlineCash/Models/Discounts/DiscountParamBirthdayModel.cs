using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models.Discounts
{
    public class DiscountParamBirthdayModel:DiscountParamModel
    {
        public int DayEnable { get; set; }
        public int DiscountPercent { get; set; }
    }
}
