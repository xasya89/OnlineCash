using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.DataModels
{
    public class StocktakingSendDataModel
    {
        public DateTime Create { get; set; }
        public Guid Uuid { get; set; }
        public decimal CashMoney { get; set; }
        public List<StocktakingGroupSendDataModel> Groups { get; set; } = new List<StocktakingGroupSendDataModel>();
    }

    public class StocktakingGroupSendDataModel
    {
        public string Name { get; set; }
        public List<StocktakingGoodSendDataModel> Goods { get; set; } = new List<StocktakingGoodSendDataModel>();
    }
    public class StocktakingGoodSendDataModel
    {
        public Guid Uuid { get; set; }
        public double CountFact { get; set; }
    }
}
