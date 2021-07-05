using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class StocktakingGood
    {
        public int Id { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public double Count { get; set; }
        public double CountDB { get; set; } = 0;
        public double CountFact { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public int StockTakingGroupId { get; set; }
        public StockTakingGroup StockTakingGroup { get; set; }
    }
}
