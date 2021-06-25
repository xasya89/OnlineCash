using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class Cashier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Inn { get; set; }
        public string PinCode { get; set; }
        public bool IsBlocked { get; set; } = false;
        public List<Shift> Shifts { get; set; }
    }
}
