using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class Buyer
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Name { get; set; }
    }
}
