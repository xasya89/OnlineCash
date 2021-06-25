using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMK.ViewModels
{
    public class GoodPriceInShop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BarCode { get; set; }
        public string Article { get; set; }
        public decimal Price { get; set; }
    }
}
