using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.DataModels
{
    public class WriteofSynchDataModel
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public DateTime DateCreate { get; set; }
        public string Note {  get; set; }
        public List<WriteofGoodSynchDataModel> Goods { get; set; } = new List<WriteofGoodSynchDataModel>();
    }

    public class WriteofGoodSynchDataModel
    {
        public Guid Uuid { get; set; }
        public double Count { get; set; }
        public decimal Price { get; set; }
    }
}
