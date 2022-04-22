using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBuyer
{
    public class Buyer
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public int? TemporyPercent { get; set; }
        public int? SpecialPercent { get; set; }
        public decimal DiscountSum { get; set; }
        public decimal SumBuy { get; set; }
        public bool isBlock { get; set; } = false;
    }
}
