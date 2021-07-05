using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCash.DataBaseModels
{
    public class StockTakingGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StocktakingId { get; set; }
        public Stocktaking Stocktaking { get; set; }
        [JsonPropertyName("goods")]
        public List<StocktakingGood> StocktakingGoods { get; set; } = new List<StocktakingGood>();
    }
}
