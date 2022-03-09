using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyersServiceApp.Models
{
    public class Buyer
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public DiscountType DiscountType { get; set; } = DiscountType.Default;
        public int? DiscountPercant { get; set; }
        public decimal DiscountSum { get; set; }
        public decimal SumBuy { get; set; }
        public bool isBlock { get; set; } = false;
    }
}
