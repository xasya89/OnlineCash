using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class GoodCountAnalyse
    {
        public int GoodId { get; set; }
        public string GoodName { get; set; }
        public Dictionary<DateTime, GoodCountAnalyseDay> CountOfDays { get; set; } = new Dictionary<DateTime, GoodCountAnalyseDay>();
    }
    public class GoodCountAnalyseDay
    {
        public decimal CountStocktaking { get; set; }
        public decimal CountPlus { get; set; }
        public decimal CountMinus { get; set; }
        public decimal CountEnd { get => CountStocktaking + CountPlus - CountMinus; }
        public decimal CountGoodBalance { get; set; }
        public bool CountEqual { get => CountEnd == CountGoodBalance; }
        public List<GoodCountAnalysDocument> Documents { get; set; } = new List<GoodCountAnalysDocument>();
    }
    public class GoodCountAnalysDocument
    {
        public string DocNum { get; set; }
        public string DocName { get; set; }
        public decimal Count { get; set; }
    }
}
