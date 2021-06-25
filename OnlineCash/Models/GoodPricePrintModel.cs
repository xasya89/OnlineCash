using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class GoodPricePrintModel
    {
        public List<int> Shops { get; set; } = new List<int>();
        public List<int> Goods { get; set; } = new List<int>();
    }
}
