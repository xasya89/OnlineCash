using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models.CashBox
{
    public class CashBoxCheckSellGoodModel
    {
        public Guid Uuid { get; set; }
        public decimal Count { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
    }

    public class CashBoxCheckSellModel
    {
        public DateTime Create { get; set; }
        public decimal SumCash { get; set; }
        public decimal SumElectron { get; set; }
        public decimal SumDiscount { get; set; }
        public List<CashBoxCheckSellGoodModel> Goods { get; set; } = new List<CashBoxCheckSellGoodModel>();
    }
}
