using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;

namespace OnlineCash.Models.GoodPrint
{
    public class SelectionGoodsModel
    {
        public IEnumerable<Shop> Shops;
        public IEnumerable<GoodGroup> Groups;
        public SelectionGoodsModel(IEnumerable<Shop> shops, IEnumerable<GoodGroup> groups)
        {
            Shops = shops;
            Groups = groups;
        }
    }
}
