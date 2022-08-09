using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCashRmk.Models
{
    public class CheckPayment
    {
        public int Id { get; set; }
        public int CheckSellId { get; set; }
        [JsonIgnore]
        public CheckSell CheckSell { get; set; }
        public TypePayment TypePayment { get; set; }
        public string TypePaymentStr
        {
            get => TypePayment.GetDisplay();
        }
        public decimal Income { get; set; }
        public decimal Sum { get; set; }
        public decimal Retturn { get; set; }
    }
}
