using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class NewGoodFromCash
    {
        public int Id { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public bool Processed { get; set; } = false;
        public bool IsNewGood { get; set; } = false;
    }
}
