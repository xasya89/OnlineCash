using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class WriteofSynchModel
    {
        public Guid Uuid { get; set; }
        public DateTime DateCreate { get; set; }
        public string Note { get; set; }
        /*
        public decimal SumAll { get; set; }
        public double CountAll { get; set; }
        */
        public List<WriteofSynchGoodModel> Goods { get; set; } = new List<WriteofSynchGoodModel>();
    }

    public class WriteofSynchGoodModel
    {
        public Guid Uuid { get; set; }
        public decimal Price { get; set; }
        public decimal Count { get; set; }
        
        //public decimal Sum { get => (decimal)Count * Price; }
    }
}
