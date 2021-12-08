using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models.Discounts
{
    public class DiscountParamModel
    {
        public Guid Uuid { get; set; } = new Guid();
        public bool IsEnable { get; set; } = true;
    }
}
