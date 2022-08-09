using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class WriteofGood
    {
        public int Id { get; set; }
        public int WriteofId { get; set; }
        public Writeof Writeof { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public string GoodName { get => Good?.Name; }
        public string GoodUnit { get => Good?.UnitDescription; }
        public double Count { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get => (decimal)Count * Price; }
    }
}
