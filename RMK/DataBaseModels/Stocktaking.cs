using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace RMK.DataBaseModels
{
    public class Stocktaking
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public DateTime Create { get; set; } = DateTime.Now;
        public int ShopId { get; set; }
        public bool isComplite { get; set; }
        public bool isSending { get; set; }
        public List<StocktakingGood> StocktakingGoods { get; set; } = new List<StocktakingGood>();
    }
}
