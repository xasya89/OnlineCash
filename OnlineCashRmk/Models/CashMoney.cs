using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class CashMoney
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public DateTime Create { get; set; }
        public TypeCashMoneyOpertaion TypeOperation { get; set; }
        public decimal Sum { get; set; }
        public string Note { get; set; }
    }
}
