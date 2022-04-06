using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCash.DataBaseModels
{
    public class Arrival
    {
        public int Id { get; set; }
        public DocumentStatus Status { get; set; } = DocumentStatus.New;
        public string Num { get; set; }
        public DateTime DateArrival { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public List<ArrivalGood> ArrivalGoods { get; set; }
        public decimal SumArrival { get; set; }
        public decimal SumNds { get; set; }
        public decimal SumSell { get; set; }
        public List<ArrivalPayment> ArrivalPayments { get; set; } = new List<ArrivalPayment>();
        public decimal SumPayments { get; set; }
        public bool isSuccess { get; set; } = false;
    }
}
