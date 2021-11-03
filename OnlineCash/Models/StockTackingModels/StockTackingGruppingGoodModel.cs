using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models.StockTackingModels
{
    public class StockTackingGruppingGoodModel
    {
        public int GoodId { get; set; }
        public string GoodName { get; set; }
        public double CountDb { get; set; }
        public double CountFact { get; set; }
        public decimal Price { get; set; }
        public decimal SumDb { get => Math.Round((decimal)CountDb * Price, 2); }
        public decimal SumFact { get => Math.Round((decimal)CountFact * Price, 2); }
        public decimal SumDiff { get => SumFact - SumDb; }
    }
}
