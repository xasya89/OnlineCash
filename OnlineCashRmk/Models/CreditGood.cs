using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class CreditGood
    {
        public int Id { get; set; }
        public double Count { get; set; }
        public decimal Cost { get; set; }
        public int GoodId { get; set; }
        public Guid? Uuid { get => Good?.Uuid; }
        public Good Good { get; set; }
        public int CreditId { get; set; }
        public decimal Sum { get => (decimal)Count * Cost; }
        [JsonIgnore]
        public Credit Credit { get; set; }
    }
}
