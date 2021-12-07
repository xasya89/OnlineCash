using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Models.BuyerSynchronization
{
    public class BuyerSynchModel
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; } = null;
        public string DiscountCardNum { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal SumBuy { get; set; }
    }
}
