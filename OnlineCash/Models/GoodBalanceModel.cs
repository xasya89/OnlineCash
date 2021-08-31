using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class GoodBalanceModel
    {
        public int ShopId { get; set; }
        public int GoodId { get; set; }
        public double Count { get; set; }
    }
}
