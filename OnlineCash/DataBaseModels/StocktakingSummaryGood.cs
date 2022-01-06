using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class StocktakingSummaryGood
    {
        public int Id { get; set; }
        public int StocktakingId { get; set; }
        public Stocktaking Stocktaking { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public decimal CountDb { get; set; }
        public decimal CountFact { get; set; }
        public decimal Price { get; set; }
    }
}
