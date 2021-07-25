using System.Text.Json.Serialization;

namespace OnlineCash.Models
{
    public class PaymentListSaveModel
    {
        public int ArrivalId { get; set; }
        public int BankId { get; set; }
        public decimal Sum { get; set; }
    }
}
