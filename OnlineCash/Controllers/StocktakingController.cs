using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using Microsoft.AspNetCore.Authorization;

namespace OnlineCash.Controllers
{
    [Authorize]
    public class StocktakingController : Controller
    {
        shopContext db;
        ILogger<StocktakingController> logger;
        public StocktakingController(shopContext db, ILogger<StocktakingController> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Shops = await db.Shops.ToListAsync();
            return View(await db.Stocktakings.Include(s => s.Shop).OrderBy(s => s.Create).OrderByDescending(s=>s.Create).ToListAsync());
        }

        public async Task<IActionResult> Create(int idShop)
        {
            ViewBag.Goods = await db.Goods.Include(g => g.GoodPrices).ToListAsync();
            var shop = await db.Shops.Where(s=>s.Id==idShop).FirstOrDefaultAsync();
            if (shop == null)
                return RedirectToAction("Index");
            int num=1;
            List<Stocktaking> stocktakingold = await db.Stocktakings.Where(s => s.ShopId == idShop).ToListAsync();
            if (stocktakingold.Count>0)
                num = stocktakingold.Max(s => s.Num)+1;
            var stocktaking = new Stocktaking { Create = DateTime.Now, Num = (int)num, Shop = shop };
            db.Stocktakings.Add(stocktaking);
            await db.SaveChangesAsync();
            stocktaking.StocktakingGoods = new List<StocktakingGood>();
            return View("Edit", stocktaking);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Goods = await db.Goods.Include(g => g.GoodPrices).ToListAsync();
            var stocktaking = await db.Stocktakings.Include(s => s.Shop).Include(s => s.StocktakingGoods).ThenInclude(sg=>sg.Good).Where(s=>s.Id==id).FirstOrDefaultAsync();
            return View(stocktaking);
        }
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Models.StockTakingSaveModel model)
        {
            Stocktaking stocktaking = await db.Stocktakings.Include(s => s.StocktakingGoods).Where(s => s.Id == model.id).FirstOrDefaultAsync();
            stocktaking.isSuccess = model.isSuccess;
            //add goods in inventiry
            foreach(var sg in model.Goods.Where(g=>g.id==-1).ToList())
            {
                var good = await db.Goods.Include(g=>g.GoodPrices).Where(g => g.Id == sg.idGood).FirstOrDefaultAsync();
                StocktakingGood stocktakingGood = new StocktakingGood
                {
                    Stocktaking = stocktaking,
                    Good = good,
                    Count = 0,
                    CountDB = 0,
                    CountFact = sg.CountFact,
                    Price = good.GoodPrices.Where(p => p.ShopId == stocktaking.ShopId).FirstOrDefault().Price
                };
                stocktaking.StocktakingGoods.Add(stocktakingGood);
            }
            foreach(var sg in model.Goods.Where(g=>g.id!=-1).ToList())
            {
                StocktakingGood stocktakingGood = await db.StocktakingGoods.Where(g => g.Id == sg.id).FirstOrDefaultAsync();
                stocktakingGood.CountFact = sg.CountFact;
            }
            //Set count good in goodbalance
            var goodbalances = await db.GoodBalances.Include(b=>b.Good).Include(b=>b.Shop).ToListAsync();
            foreach(var good in model.Goods)
            {
                var goodbalance = goodbalances.Where(b => b.GoodId == good.idGood & b.ShopId == stocktaking.ShopId).FirstOrDefault();
                if (goodbalance == null)
                {
                    var gb = new GoodBalance
                    {
                        ShopId = stocktaking.ShopId,
                        GoodId = good.idGood,
                        Count = good.CountFact
                    };
                    db.GoodBalances.Add(gb);
                }
                else
                    goodbalance.Count = good.CountFact;
            }

            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
