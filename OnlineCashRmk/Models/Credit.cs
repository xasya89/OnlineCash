using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCashRmk.Models
{
    public class Credit
    {
        public int Id { get; set; }
        public string Creditor { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public decimal Sum { get; set; }
        public decimal SumDiscont { get; set; } = 0;
        public decimal SumAll { get; set; }
        public decimal SumCredit { get; set; }
        public List<CreditGood> CreditGoods { get; set; } = new List<CreditGood>();
        public List<CreditPayment> CreditPayments { get; set; } = new List<CreditPayment>();
        public int? ShiftId { get; set; }
        public Shift Shift { get; set; }
        public bool isSynch { get; set; }
    }
}
