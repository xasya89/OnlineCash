using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class ArrivalPayment
    {
        public int Id { get; set; }
        public int ArrivalId { get; set; }
        public Arrival Arrival { get; set; }
        public DateTime DatePayment { get; set; } = DateTime.Now;
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public decimal Sum { get; set; }
    }
}
