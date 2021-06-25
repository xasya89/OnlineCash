using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class GoodPrice
    {
        public int Id { get; set; }
        public int GoodId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public Good Good { get; set; }
        public int ShopId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public Shop Shop { get; set; }
        public decimal Price { get; set; }
    }
}
