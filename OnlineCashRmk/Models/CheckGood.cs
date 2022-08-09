using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCashRmk.Models
{
    public class CheckGood
    {
        public int Id { get; set; }
        public double Count { get; set; }
        public decimal Cost { get; set; }
        public int GoodId { get; set; }
        public Guid? Uuid { get => Good?.Uuid; }
        public string GoodName { get => Good?.Name; }
        public Good Good { get; set; }
        public int CheckSellId { get; set; }
        public decimal Sum { get => (decimal)Count * Cost; }
        [JsonIgnore]
        public CheckSell CheckSell { get; set; }
    }
}
