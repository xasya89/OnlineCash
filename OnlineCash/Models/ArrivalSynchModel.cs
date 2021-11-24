using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class ArrivalSynchModel
    {
        public Guid Uuid { get; set; }
        public string Num { get; set; }
        public DateTime DateArrival { get; set; }
        public int SupplierId { get; set; }
        public List<ArrivalGoodSynchModel> ArrivalGoods { get; set; } = new List<ArrivalGoodSynchModel>();
    }

    public class ArrivalGoodSynchModel
    {
        public Guid GoodUuid { get; set; }
        public decimal Price { get; set; }
        public decimal Count { get; set; }
    }
}
