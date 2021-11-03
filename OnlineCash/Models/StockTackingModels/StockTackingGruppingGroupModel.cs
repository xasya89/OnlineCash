using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models.StockTackingModels
{
    public class StockTackingGruppingGroupModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public decimal SumDb { get => Goods.Sum(g => g.SumDb); }
        public decimal SumFact { get => Goods.Sum(g => g.SumFact); }
        public decimal SumDiff { get => Goods.Sum(g => g.SumDiff); }
        public List<StockTackingGruppingGoodModel> Goods = new List<StockTackingGruppingGoodModel>();
    }
}
