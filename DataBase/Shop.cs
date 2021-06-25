using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GoodPrice> GoodPrices { get; set; }
        public List<Shift> Shifts { get; set; }
    }
}
