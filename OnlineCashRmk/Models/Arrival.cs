using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCashRmk.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; }
        public List<Arrival> Arrivals { get; set; }
    }
    public class Arrival
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; } = new Guid();
        public string Num { get; set; }
        public DateTime DateArrival { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public List<ArrivalGood> ArrivalGoods { get; set; } = new List<ArrivalGood>();
    }
    public class ArrivalGood
    {
        public int Id { get; set; }
        public int ArrivalId { get; set; }
        public Arrival Arrival { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public decimal Price { get; set; }
        public decimal Count { get; set; }
        public string Nds { get; set; } = "Без ндс";
        public decimal Sum { get => Price * Count; }
        public DateTime? ExpiresDate { get; set; }
    }
}
