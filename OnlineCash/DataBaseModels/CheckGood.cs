using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCash.DataBaseModels
{
    public class CheckGood
    {
        public int Id { get; set; }
        public decimal Count { get; set; }
        public decimal Price { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public decimal Sum { get => Count * Price; }
        public int CheckSellId { get; set; }
        [JsonIgnore]
        public CheckSell CheckSell { get; set; }
    }
}
