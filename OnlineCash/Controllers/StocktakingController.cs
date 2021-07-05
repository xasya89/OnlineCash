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
            var shop = await db.Shops.Where(s => s.Id == idShop).FirstOrDefaultAsync();
            if (shop == null)
                return RedirectToAction("Index");
            int num = 1;
            List<Stocktaking> stocktakingold = await db.Stocktakings.Where(s => s.ShopId == idShop).ToListAsync();
            if (stocktakingold.Count > 0)
                num = stocktakingold.Max(s => s.Num) + 1;
            var stocktaking = new Stocktaking { Create = DateTime.Now, Num = (int)num, Shop = shop };
            db.Stocktakings.Add(stocktaking);
            var group = new StockTakingGroup { Stocktaking = stocktaking, Name = "Без группы" };
            db.StockTakingGroups.Add(group);
            await db.SaveChangesAsync();
            return View("Edit", stocktaking);
        }

        public async Task<IActionResult> Edit(int id) 
        {
            ViewBag.Goods = await db.Goods.Include(g => g.GoodPrices).ToListAsync();
            var stocktaking = await db.Stocktakings.Include(s => s.Shop).Include(s => s.StockTakingGroups).ThenInclude(gr=>gr.StocktakingGoods).ThenInclude(sg=>sg.Good).Where(s=>s.Id==id).FirstOrDefaultAsync();
            return View(stocktaking);
        }
        
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Models.StockTakingSaveModel model)
        {
            Stocktaking stocktaking = await db.Stocktakings.Include(s => s.StockTakingGroups).ThenInclude(gr=>gr.StocktakingGoods).Where(s => s.Id == model.id).FirstOrDefaultAsync();
            bool isSuccessOld = stocktaking.isSuccess;
            stocktaking.isSuccess = model.isSuccess;
            stocktaking.Status = model.isSuccess ? DocumentStatus.Confirm : stocktaking.Status;
            foreach(var sgr in model.Groups)
            {
                StockTakingGroup group = null;
                if(sgr.Id==-1)
                {
                    group = new StockTakingGroup
                    {
                        Name = sgr.Name,
                        Stocktaking = stocktaking
                    };
                    db.StockTakingGroups.Add(group);
                }
                else
                {
                    group = await db.StockTakingGroups.Where(gr => gr.Id == sgr.Id).FirstOrDefaultAsync();
                    if (group == null)
                        return BadRequest();
                    group.Name = sgr.Name;
                };
                //Добавим новые товары
                foreach (var sg in sgr.Goods.Where(g => g.id == -1).ToList())
                {
                    var good = await db.Goods.Include(g => g.GoodPrices).Where(g => g.Id == sg.idGood).FirstOrDefaultAsync();
                    StocktakingGood stocktakingGood = new StocktakingGood
                    {
                        StockTakingGroup = group,
                        Good = good,
                        Count = 0,
                        CountDB = 0,
                        CountFact = sg.CountFact,
                        Price = good.GoodPrices.Where(p => p.ShopId == stocktaking.ShopId).FirstOrDefault().Price
                    };
                    db.StocktakingGoods.Add(stocktakingGood);
                }
                //Изменим количество
                foreach (var sg in sgr.Goods.Where(g => g.id != -1).ToList())
                {
                    StocktakingGood stocktakingGood = await db.StocktakingGoods.Where(g => g.Id == sg.id).FirstOrDefaultAsync();
                    stocktakingGood.CountFact = sg.CountFact;
                }
            }
            await db.SaveChangesAsync();
            //Set count good in goodbalance
            if (model.isSuccess & !isSuccessOld)
            {
                var goodbalances = await db.GoodBalances.Include(b => b.Good).ThenInclude(b=>b.GoodPrices.Where(p=>p.ShopId==stocktaking.ShopId)).Include(b => b.Shop).ToListAsync();
                foreach(var group in model.Groups)
                    foreach (var good in group.Goods)
                    {
                        var goodbalance = goodbalances.Where(b => b.GoodId == good.idGood & b.ShopId == stocktaking.ShopId).FirstOrDefault();
                        if (goodbalance != null)
                            goodbalance.Count = good.CountFact;
                    }
                stocktaking = await db.Stocktakings.Include(s => s.StockTakingGroups).ThenInclude(gr => gr.StocktakingGoods).Where(s => s.Id == model.id).FirstOrDefaultAsync();

                var stocktakingGoods = new List<StocktakingGood>();
                foreach (var group in stocktaking.StockTakingGroups)
                    foreach (var good in group.StocktakingGoods)
                        stocktakingGoods.Add(good);
                var groupfirst = await db.StockTakingGroups.Where(gr => gr.StocktakingId == model.id).FirstOrDefaultAsync();
                foreach (var goodbalance in goodbalances)
                    if (stocktakingGoods.Count(s => s.GoodId == goodbalance.GoodId) == 0 & goodbalance.Count!=0)
                    {

                        db.StocktakingGoods.Add(new StocktakingGood
                        {
                            StockTakingGroup=groupfirst,
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
            if (stocktaking.Status != DocumentStatus.Confirm)
                stocktaking.Status = DocumentStatus.Remove;
            await db.SaveChangesAsync();
            return await Index();
        }
    }
}
