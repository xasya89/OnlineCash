using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;

namespace OnlineCash.Models
{
    public class PaymentListSaveModel
    {
        [Required]
        public int ArrivalId { get; set; }
        [Required]
        public int BankId { get; set; }
        [Required]
        public DateTime DatePayment { get; set; }
        [Required]
        public decimal Sum { get; set; }
    }
}
