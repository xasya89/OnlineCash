using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class DiscountCard
    {
        public int Id { get; set; }
        public string Num { get; set; }
        public bool isFree { get; set; } = true;
    }
}
