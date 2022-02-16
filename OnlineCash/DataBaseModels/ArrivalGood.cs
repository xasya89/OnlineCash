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
        public decimal Sum { get
            {
                decimal sum = (decimal)Count * Price;
                switch (Nds)
                {
                    case "20%":
                        //return Sum / 120 * 20;
                        return Math.Round(sum + sum * 0.2M, 2);
                        break;
                    case "10%":
                        //return Sum / 110 * 10;
                        return Math.Round(sum + sum * 0.1M, 2);
                        break;
                    case "0%":
                        return Math.Round(sum,2);
                        break;
                    default:
                        return Math.Round(sum, 2);
                        break;
                }
            }
        }
        public decimal SumNds
        {
            get
            {
                decimal sum = (decimal)Count * Price;
                switch (Nds)
                {
                    case "20%":
                        //return Sum / 120 * 20;
                        return Math.Round(sum * 0.2M,2);
                        break;
                    case "10%":
                        //return Sum / 110 * 10;
                        return Math.Round(sum * 0.1M, 2);
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
        public decimal SumSell { get => Math.Round((decimal)Count * PriceSell, 2); }
        public DateTime? ExpiresDate { get; set; }
        public int ArrivalId { get; set; }
        public Arrival Arrival { get; set; }
    }
}
