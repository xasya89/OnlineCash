using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models.Discounts
{
    public class DiscountParamWeeksModel
    {
        public int? MonDiscountPercent { get; set; }
        public int? TueDiscountPercent { get; set; }
        public int? WedDiscountPercent { get; set; }
        public int? ThuDiscountPercent { get; set; }
        public int? FriDiscountPercent { get; set; }
        public int? SatDiscountPercent { get; set; }
        public int? SunDiscountPercent { get; set; }
    }
}
