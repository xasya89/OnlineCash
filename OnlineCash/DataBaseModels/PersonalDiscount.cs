using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class PersonalDiscount
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public int BuyerId { get; set; }
        public Buyer Buyer { get; set; }
        public int? DiscountPercent { get; set; }
        public decimal? DiscountSum { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateAccept { get; set; }
        public string Note { get; set; }
    }
}
