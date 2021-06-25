using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OnlineCash.ViewModels
{
    public class ShopPriceViewModel
    {
        public int idShop { get; set; }
        public int idPrice { get; set; }
        public string ShopName { get; set; }
        public decimal Price { get; set; }
    }
}
