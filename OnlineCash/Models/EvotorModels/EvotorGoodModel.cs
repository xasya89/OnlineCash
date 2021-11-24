using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models.EvotorModels
{
    public class EvotorGoodModel
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public List<string> BarCodes { get; set; } = new List<string>();
        public decimal? Price { get; set; }
        public string MeasureName { get; set; }
        public bool? AllowToSell { get; set; }
    }
}
