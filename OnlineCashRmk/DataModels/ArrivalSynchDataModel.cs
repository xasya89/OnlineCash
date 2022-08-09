using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.DataModels
{
    public class ArrivalSynchDataModel
    {
        public Guid Uuid { get; set; } = new Guid();
        public string Num { get; set; }
        public DateTime DateArrival { get; set; }
        public int SupplierId { get; set; }
        public List<ArrivalGoodSynchDataModel> ArrivalGoods { get; set; } = new List<ArrivalGoodSynchDataModel>();
    }
    public class ArrivalGoodSynchDataModel
    {
        public Guid GoodUuid { get; set; }
        public decimal Price { get; set; }
        public decimal Count { get; set; }
        public string Nds { get; set; }
        public DateTime? ExpiresDate { get; set; }
    }
}
