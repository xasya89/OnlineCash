using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models.StockTackingModels
{
    public class StockTackingGruppingModel
    {
        public int NumDoc { get; set; }
        public DateTime DateDoc { get; set; }
        public string ShopName { get; set; }
        public decimal SumDb { get => Groups.Sum(gr => gr.SumDb); }
        public decimal SumFact { get => Groups.Sum(gr => gr.SumFact); }
        public decimal SumDiff { get => Groups.Sum(gr => gr.SumDiff); }
        public List<StockTackingGruppingGroupModel> Groups = new List<StockTackingGruppingGroupModel>();
    }
}
