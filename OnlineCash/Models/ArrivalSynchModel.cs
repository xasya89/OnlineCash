using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class ArrivalSynchModel
    {
        public Guid Uuid { get; set; }
        public string Num { get; set; }
        public DateTime DateArrival { get; set; }
        public int SupplierId { get; set; }
        public List<ArrivalGoodSynchModel> ArrivalGoods { get; set; } = new List<ArrivalGoodSynchModel>();
    }

    public class ArrivalGoodSynchModel
    {
        public Guid GoodUuid { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSell { get; set; }
        public decimal Count { get; set; }
        public decimal Sum
        {
            get
            {
                decimal sum = Count * Price;
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
                        return 0;
                        break;
                    default:
                        return 0;
                        break;
                };
            }
        }

        public string Nds { get; set; }
        public decimal SumNds
        {
            get
            {
                decimal sum = Count * Price;
                switch (Nds)
                {
                    case "20%":
                        //return Sum / 120 * 20;
                        return Math.Round(sum * 0.2M, 2);
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
        public DateTime? ExpiresDate { get; set; }
    }
}
