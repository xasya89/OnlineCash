using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMK.Models
{
    public class CheckSellModel
    {
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public bool IsElectron { get; set; }
        public decimal Sum { get; set; }
        public decimal SumDiscont { get; set; } = 0;
        public decimal SumAll { get; set; }
        public List<CheckGoodModel> Goods { get; set; } = new List<CheckGoodModel>();
    }
}
