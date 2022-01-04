using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class StockTakingSaveModel
    {
        public int id { get; set; }
        public bool isSuccess { get; set; }
        public DateTime Create { get; set; }
        public List<StocktakingGroupModel> Groups { get; set; }
    }
}
