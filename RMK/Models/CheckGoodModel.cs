using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMK.Models
{
    public class CheckGoodModel
    {
        public Guid Uuid { get; set; }
        public double Count { get; set; }
        public decimal Cost { get; set; }
    }
}
