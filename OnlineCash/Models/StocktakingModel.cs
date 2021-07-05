using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class StocktakingModel
    {
        public int Num { get; set; }
        public DateTime Create { get; set; } = DateTime.Now;
        public int ShopId { get; set; }
        public List<StocktakingGroupModel> Groups { get; set; } = new List<StocktakingGroupModel>();
    }
}
