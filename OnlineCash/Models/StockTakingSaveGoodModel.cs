using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class StockTakingSaveGoodModel
    {
        public int id { get; set; }
        public int idGood{get;set;}
        public Guid Uuid { get; set; }
        public double CountFact { get; set; }
    }
}
