using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class Buyer
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string Phone { get; set; }
        public int DiscountCardId { get; set; }
        public DiscountCard DiscountCard { get; set; }
        public DateTime? Birthday { get; set; }
        public DiscountType DiscountType { get; set; } = DiscountType.Default;
        public int? DiscountPercant { get; set; }
        public decimal DiscountSum { get; set; }
        public decimal SumBuy { get; set; }
        public bool isBlock { get; set; } = false;
        public List<CheckSell> CheckSells { get; set; }
        public List<PersonalDiscount> PersonalDiscounts { get; set; }
    }
}
