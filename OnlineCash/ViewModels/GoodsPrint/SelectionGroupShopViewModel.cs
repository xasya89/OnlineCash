using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Models.GoodPrint;
using OnlineCash.DataBaseModels;

namespace OnlineCash.ViewModels.GoodsPrint
{
    public class SelectionGroupShopViewModel
    {
        shopContext db;
        public SelectionGroupShopViewModel(shopContext db)
        {
            this.db = db;
        }
        public async Task<List<GoodGroup>> GetGoods(SelectionGroupShopModel selections) =>
            await db.GoodGroups.Where(gr => selections.idGroups.Contains(gr.Id))
                .Include(gr => gr.Goods)
                .ThenInclude(g => g.GoodPrices.Where(p => selections.idShops.Contains(p.ShopId)))
                .ToListAsync();
    }
}
