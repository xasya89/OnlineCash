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
            return View("Index",await db.Stocktakings.Include(s => s.Shop).OrderBy(s => s.Create).OrderByDescending(s=>s.Create).ToListAsync());
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
            stocktaking.StocktakingGoods = new List<StocktakingGood>();
            /*
            var goodbalances= await db.GoodBalances.Include(gb=>gb.Good).ThenInclude(gb=>gb.GoodPrices.Where(p=>p.ShopId==shop.Id)).Where(gb => gb.ShopId == shop.Id & gb.Count!=0).ToListAsync();
            foreach (var goodbalance in goodbalances)
            {
                StocktakingGood stocktakingGood = new StocktakingGood
                {
                    Good = goodbalance.Good,
                    Count = 0,
                    CountDB = goodbalance.Count,
                    CountFact = 0,
                    Price = goodbalance.Good.GoodPrices.FirstOrDefault().Price,
                    Stocktaking = stocktaking
                };
                db.StocktakingGoods.Add(stocktakingGood);
                stocktaking.StocktakingGoods.Add(stocktakingGood);
            };
            */
            await db.SaveChangesAsync();

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
            bool isSuccessOld = stocktaking.isSuccess;
            stocktaking.isSuccess = model.isSuccess;
            //Добавим новые товары
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
                db.StocktakingGoods.Add(stocktakingGood);
                //stocktaking.StocktakingGoods.Add(stocktakingGood);
            }
            //Изменим количество
            foreach(var sg in model.Goods.Where(g=>g.id!=-1).ToList())
            {
                StocktakingGood stocktakingGood = await db.StocktakingGoods.Where(g => g.Id == sg.id).FirstOrDefaultAsync();
                stocktakingGood.CountFact = sg.CountFact;
            }
            await db.SaveChangesAsync();
            //Set count good in goodbalance
            if (model.isSuccess & !isSuccessOld)
            {
                var goodbalances = await db.GoodBalances.Include(b => b.Good).ThenInclude(b=>b.GoodPrices.Where(p=>p.ShopId==stocktaking.ShopId)).Include(b => b.Shop).ToListAsync();

                foreach (var good in model.Goods)
                {
                    var goodbalance = goodbalances.Where(b => b.GoodId == good.idGood & b.ShopId == stocktaking.ShopId).FirstOrDefault();
                    if (goodbalance != null)
                        goodbalance.Count = good.CountFact;
                }

                var stocktakingGoods = await db.StocktakingGoods.Include(s => s.Good).Where(s => s.StocktakingId == stocktaking.Id).ToListAsync();
                foreach (var goodbalance in goodbalances)
                    if (stocktakingGoods.Count(s => s.GoodId == goodbalance.GoodId) == 0 & goodbalance.Count!=0)
                    {
                        db.StocktakingGoods.Add(new StocktakingGood
                        {
                            Stocktaking = stocktaking,
                            Good = goodbalance.Good,
                            Count = 0,
                            CountDB = goodbalance.Count,
                            CountFact = 0,
                            Price = goodbalance.Good.GoodPrices.FirstOrDefault().Price
                        });
                        goodbalance.Count = 0;
                    };
                //Теперь учтем продажи после даты выполнения инверторизации
                var sells = await db.Shifts.Include(s => s.CheckSells).ThenInclude(c => c.CheckGoods).ThenInclude(g => g.Good).Where(s => s.ShopId == stocktaking.ShopId & s.Start >= stocktaking.Create).ToListAsync();
                foreach(var sell in sells)
                    foreach(var check in sell.CheckSells)
                        foreach(var good in check.CheckGoods)
                        {
                            var goodbalance = goodbalances.Where(gb => gb.GoodId == good.GoodId).FirstOrDefault();
                            if (goodbalance != null)
                                goodbalance.Count -= good.Count;
                        }    
            }

            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(int id)
        {
            var stocktaking = await db.Stocktakings.Where(s => s.Id == id).FirstOrDefaultAsync();
            db.Stocktakings.Remove(stocktaking);
            await db.SaveChangesAsync();
            return await Index();
        }
    }
}
