using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;

namespace OnlineCash.ViewModels
{
    public class GoodViewModel:Good
    {
        public List<ShopPriceViewModel> PriceShops { get; set; } = new List<ShopPriceViewModel>();
    }
}
