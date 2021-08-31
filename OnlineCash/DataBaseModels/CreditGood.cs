using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
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
    }
}
