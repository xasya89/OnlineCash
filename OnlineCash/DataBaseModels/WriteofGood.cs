using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class WriteofGood
    {
        public int Id { get; set; }
        public int WriteofId { get; set; }
        public Writeof Writeof { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public double Count { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get => Math.Round((decimal)Count * Price, 2); }
    }
}
