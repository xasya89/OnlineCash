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
        IGoodBalanceService _goodBalanceService;
        public StocktakingController(shopContext db, ILogger<StocktakingController> logger, IStockTackingService stockTackingService, IGoodBalanceService goodBalanceService)
        {
            this.db = db;
            this.logger = logger;
            this.stockTackingService = stockTackingService;
            _goodBalanceService = goodBalanceService;
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

        public async Task<IActionResult> CreateFromSuppliers(int idShop)
        {
            var suppliers = await db.Suppliers.OrderBy(s => s.Name).ToListAsync();
            var goods = await db.Goods.Include(g => g.GoodPrices).Include(g => g.Supplier).ToListAsync();
            ViewBag.Goods = goods;
            var shop = await db.Shops.Where(s => s.Id == idShop).FirstOrDefaultAsync();
            if (shop == null)
                return RedirectToAction("Index");
            int num = 1;
            List<Stocktaking> stocktakingold = await db.Stocktakings.Where(s => s.ShopId == idShop).ToListAsync();
            if (stocktakingold.Count > 0)
                num = stocktakingold.Max(s => s.Num) + 1;
            var stocktaking = new Stocktaking { Num = num, Shop = shop, Create = DateTime.Now, Start = DateTime.Now };
            db.Stocktakings.Add(stocktaking);
            var goodbalance = await db.GoodBalances.ToListAsync();
            foreach(var supplier in suppliers)
            {
                var group = new StockTakingGroup { Name = supplier.Name, Stocktaking = stocktaking };
                db.StockTakingGroups.Add(group);
                var goodsSupplier = goods.Where(g => g.SupplierId == supplier.Id & g.IsDeleted == false).ToList();
                foreach(var good in goodsSupplier)
                {
                    var gb = goodbalance.Where(g => g.GoodId == good.Id).FirstOrDefault();
                    if (gb != null)
                    {
                        var stGood = new StocktakingGood
                        {
                            StockTakingGroup=group,
                            GoodId = good.Id,
                            CountDB = gb.Count,
                            Price = good.GoodPrices.Where(p => p.ShopId == idShop).FirstOrDefault().Price
                        };
                        db.StocktakingGoods.Add(stGood);
                        stGood.StockTakingGroupId = group.Id;
                        group.StocktakingGoods.Add(stGood);
                    }
                }
                group.StocktakingId = stocktaking.Id;
                stocktaking.StockTakingGroups.Add(group);
            }
            var goodNotSupplier = goods.Where(g => g.Supplier == null).ToList();
            if (goodNotSupplier.Count > 0)
            {
                var group = new StockTakingGroup { Name = "Без поставщика", Stocktaking = stocktaking };
                db.StockTakingGroups.Add(group);
                foreach(var good in goodNotSupplier)
                {
                    var gb = goodbalance.Where(g => g.GoodId == good.Id).FirstOrDefault();
                    if (gb != null)
                    {
                        var stGood = new StocktakingGood
                        {
                            StockTakingGroup = group,
                            GoodId = good.Id,
                            CountDB = gb.Count,
                            Price = good.GoodPrices.Where(p => p.ShopId == idShop).FirstOrDefault().Price
                        };
                        db.StocktakingGoods.Add(stGood);
                        stGood.StockTakingGroupId = group.Id;
                        //group.StocktakingGoods.Add(stGood);
                    }
                }
                
                group.StocktakingId = stocktaking.Id;
                //stocktaking.StockTakingGroups.Add(group);
            }

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
            try
            {
                await stockTackingService.Save(model);
                return Ok();
            }
            catch (Exception)
            {

            }
            return BadRequest();
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
        public async Task<IActionResult> Summary(int id, int? goodGroupId)
        {
            var goodGroups = new List<GoodGroup>() { new GoodGroup { Id = 0 } };
            goodGroups.AddRange(await db.GoodGroups.OrderBy(gr => gr.Name).ToListAsync());
            ViewBag.GoodGroups = goodGroups;
            ViewBag.GoodGroupIdSelected = goodGroupId;
            return View(await stockTackingService.GetSummary(id, goodGroupId));
        }

        public async Task<IActionResult> GetDetailGoodCountByDocs(int stocktakingId, int goodId)
            => Ok(await stockTackingService.GetDetailGoodCountByDocs(stocktakingId, goodId));
    }
}
