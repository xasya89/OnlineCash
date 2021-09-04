using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class Good
    {
        public int Id { get; set; }
        public int GoodGroupId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public GoodGroup GoodGroup { get; set; }
        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Article { get; set; }
        public string BarCode { get; set; }
        public Units Unit { get; set; }
        public decimal Price { get; set; }
        public SpecialTypes SpecialType { get; set; } = SpecialTypes.None;
        public double? VPackage { get; set; }
        public List<BarCode> BarCodes { get; set; } = new List<BarCode>();
        public List<GoodPrice> GoodPrices { get; set; }
        public List<CheckGood> CheckGoods { get; set; }
        public List<StocktakingGood> StocktakingGoods { get; set; }
        public List<GoodBalance> GoodBalances { get; set; }
        public List<ArrivalGood> ArrivalGoods { get; set; }
        public List<GoodAdded> GoodAddeds { get; set; }
        public List<WriteofGood> WriteofGoods { get; set; }
    }
}
