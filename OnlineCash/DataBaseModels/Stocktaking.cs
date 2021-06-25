using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase;
using System.Text.Json.Serialization;

namespace OnlineCash.DataBaseModels
{
    public class Stocktaking
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        public int Num { get; set; }
        public DateTime Create { get; set; } = DateTime.Now;
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public bool isSuccess { get; set; }
        public List<StocktakingGood> StocktakingGoods { get; set; }
    }
}
