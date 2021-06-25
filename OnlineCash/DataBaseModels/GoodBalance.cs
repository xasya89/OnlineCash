using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class GoodBalance
    {
        public int Id { get; set; }
        public Shop Shop { get; set; }
        public int ShopId { get; set; }
        public Good Good { get; set; }
        public int GoodId { get; set; }
        public double Count { get; set; }
    }
}
