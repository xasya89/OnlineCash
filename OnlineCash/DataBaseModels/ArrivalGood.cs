using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class ArrivalGood
    {
        public int Id { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public double Count { get; set; }
        public decimal Price { get; set; }
        public int ArrivalId { get; set; }
        public Arrival Arrival { get; set; }
    }
}
