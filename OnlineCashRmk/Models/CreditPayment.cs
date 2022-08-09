using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class CreditPayment
    {
        public int Id { get; set; }
        public DateTime DatePayment { get; set; }
        public decimal Sum { get; set; }
        public int CreditId { get; set; }
        public Credit Credit { get; set; }
    }
}
