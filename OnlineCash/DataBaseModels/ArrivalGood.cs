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
        public string Nds { get; set; }
        public decimal PriceSell { get; set; }
        public decimal Sum { get => Math.Round((decimal)Count * Price,2); }
        public decimal SumNds
        {
            get
            {
                switch (Nds)
                {
                    case "20%":
                        return Sum / 120 * 20;
                        break;
                    case "10%":
                        return Sum / 110 * 10;
                        break;
                    case "0%":
                        return 0;
                        break;
                    default:
                        return 0;
                        break;
                }
            }
        }
        public int ArrivalId { get; set; }
        public Arrival Arrival { get; set; }
    }
}
