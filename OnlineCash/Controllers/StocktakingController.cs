using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using Microsoft.AspNetCore.Authorization;
using OnlineCash.Services;
using OnlineCash.Models.StockTackingModels;

namespace OnlineCash.Controllers
{
    [Authorize]
    public class StocktakingController : Controller
    {
        shopContext db;
        ILogger<StocktakingController> logger;
        IStockTackingService stockTackingService;
        public StocktakingController(shopContext db, ILogger<StocktakingController> logger, IStockTackingService stockTackingService)
        {
            this.db = db;
            this.logger = logger;
            this.stockTackingService = stockTackingService;
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
            //Перечитаем инверторизацию
            stocktaking = await db.Stocktakings.Where(s => s.Id == model.id)
                .Include(s => s.StockTakingGroups)
                .ThenInclude(gr => gr.StocktakingGoods)
                .FirstOrDefaultAsync();
            //Добавим товары в нераспределено
            if (model.isSuccess & !isSuccessOld)
            {
                Dictionary<int, double> goodInStocktaking = new Dictionary<int, double>();
                foreach (var stGroup in stocktaking.StockTakingGroups)
                foreach (var stGood in stGroup.StocktakingGoods)
                    if (goodInStocktaking.ContainsKey(stGood.GoodId))
                        goodInStocktaking[stGood.GoodId] += stGood.CountFact;
                    else
                        goodInStocktaking.Add(stGood.GoodId,stGood.CountFact);
                var goodBalances = await db.GoodBalances
                    .Include(b=>b.Good)
                    .ThenInclude(g=>g.GoodPrices
                        .Where(p=>p.ShopId==stocktaking.ShopId))
                    .Where(gb => gb.ShopId == stocktaking.ShopId)
                    .ToListAsync();
                
                var groupNoInvertory = new StockTakingGroup { Stocktaking = stocktaking, Name = "Не учтено" };
                db.StockTakingGroups.Add(groupNoInvertory);
                foreach (var goodBalance in goodBalances)
                    if (!goodInStocktaking.ContainsKey(goodBalance.GoodId))
                    {
                        db.StocktakingGoods.Add(new StocktakingGood
                        {
                            StockTakingGroup = groupNoInvertory,
                            GoodId = goodBalance.GoodId,
                            Price = goodBalance.Good.GoodPrices.FirstOrDefault().Price,
                            Count = 0,
                            CountDB = goodBalance.Count,
                            CountFact = 0,
                        });
                        goodBalance.Count = 0;
                    }
            }
            //Изменим количесво в GoodBalance
            if (model.isSuccess & !isSuccessOld)
            {
                Dictionary<int, double> goodInStocktaking = new Dictionary<int, double>();
                foreach (var stGroup in stocktaking.StockTakingGroups)
                foreach (var stGood in stGroup.StocktakingGoods)
                    if (goodInStocktaking.ContainsKey(stGood.GoodId))
                        goodInStocktaking[stGood.GoodId] += stGood.CountFact;
                    else
                        goodInStocktaking.Add(stGood.GoodId,stGood.CountFact);
                
                //Проверим количество в системе для товаров добавленных в инверторизацию
                var goodBalances = await db.GoodBalances.Include(b=>b.Good).ThenInclude(g=>g.GoodPrices.Where(p=>p.ShopId==stocktaking.ShopId)).Where(gb => gb.ShopId == stocktaking.ShopId).ToListAsync();
                foreach (var goodBalance in goodBalances)
                    if (goodInStocktaking.ContainsKey(goodBalance.GoodId))
                        goodBalance.Count = goodInStocktaking[goodBalance.GoodId];
                    else
                        goodBalance.Count = 0;
                foreach (var goodId in goodInStocktaking.Keys)
                    if (goodBalances.Count(gb => gb.GoodId == goodId) == 0)
                        db.GoodBalances.Add(new GoodBalance
                        {
                            ShopId = stocktaking.ShopId,
                            GoodId = goodId,
                            Count = goodInStocktaking[goodId]
                        });
            }   
            //Подведем итоги инверторизации и запишем итоговые значение
            stocktaking = await db.Stocktakings.Where(s => s.Id == model.id).Include(s => s.StockTakingGroups).ThenInclude(gr => gr.StocktakingGoods).FirstOrDefaultAsync();
            stocktaking.CountDb = stocktaking.StockTakingGroups.Sum(s => s.StocktakingGoods.Sum(g => g.CountDB));
            stocktaking.CountFact = stocktaking.StockTakingGroups.Sum(s => s.StocktakingGoods.Sum(g => g.CountFact));
            stocktaking.SumDb = stocktaking.StockTakingGroups.Sum(s => s.StocktakingGoods.Sum(g =>(decimal) g.CountDB * g.Price));
            stocktaking.SumFact = stocktaking.StockTakingGroups.Sum(s => s.StocktakingGoods.Sum(g => (decimal)g.CountFact * g.Price));
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

        [HttpGet]
        public async Task<IActionResult> Print(int id)
            => View(await db.Stocktakings.Include(s => s.Shop).Include(s => s.StockTakingGroups).ThenInclude(g => g.StocktakingGoods).ThenInclude(g => g.Good).Where(s => s.Id == id).FirstOrDefaultAsync());
        
        [HttpGet("Stocktaking/DetailsGroups/{id}")]
        public async Task<IActionResult> DetailsGroups(int id) => View(await stockTackingService.GetDetailsGroups(id));

        [HttpGet("Stocktaking/Summary/{id}")]
        public async Task<IActionResult> Summary(int id) => View(await stockTackingService.GetSummary(id));

    }
}
