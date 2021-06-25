using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase;
using System.Text.Json.Serialization;

namespace RMK.DataBaseModels
{
    public class StocktakingGood
    {
        public int Id { get; set; }
        public Guid GoodUuid { get; set; }
        public int idGood { get; set; }
        public double Count { get; set; }
        public int StocktakingId { get; set; }
        [JsonIgnore]
        public Stocktaking Stocktaking { get; set; }
    }
}
