using DataBase;
using OnlineCash.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class GoodSynchModel
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Article { get; set; }
        public Units Unit { get; set; }
        public SpecialTypes SpecialType { get; set; } = SpecialTypes.None;
        public double? VPackage { get; set; }
        public List<string> Barcodes { get; set; } = new List<string>();
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
