using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.ViewModels;
using OnlineCash.DataBaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace OnlineCash.Controllers
{
    [Authorize]
    public class GoodsController : Controller
    {
        public IConfiguration configuration;
        public ILogger<GoodsController> logger;
        public shopContext db;
        public GoodsController(IConfiguration configuration, ILogger<GoodsController> logger, shopContext db)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            string find= HttpContext.Session.GetString("SearchGoods");
            ViewBag.Find = find;
            return View(await db.Goods.Where(g=>EF.Functions.Like(g.Name, $"%{find}%")).OrderBy(g => g.Name).ToListAsync());
        }

        public async Task<IActionResult> Search(string find)
        {
            find = HttpContext.Session.GetString("SearchGoods");
            ViewBag.Find = find;
            return View("Index", await db.Goods.Where(g => EF.Functions.Like(g.Name, $"%{find}%")).OrderBy(g => g.Name).ToListAsync());
        }

        public IActionResult SetSearch(string search="")
        {
            HttpContext.Session.SetString("SearchGoods", search??"");
            return Ok();
        }

        public async Task<IActionResult> PrintAll()
        {
            var model = new List<GoodViewModel>();
            var goods = await db.Goods.OrderBy(g => g.Name).ToListAsync();
            foreach (var g in goods)
                model.Add(new GoodViewModel
                {
                    Name = g.Name,
                    Article = g.Article,
                    BarCode = g.BarCode,
                    Price = g.Price
                });
            return View("PrintGood", model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var good = await db.Goods.FirstOrDefaultAsync(g => g.Id == id);
            var model = new GoodViewModel();
            model.Id = good.Id;
            model.Name = good.Name;
            model.Article = good.Article;
            model.BarCode = good.BarCode;
            model.Unit = good.Unit;
            model.Price = good.Price;
            var prices = await db.GoodPrices.Include(p => p.Shop).Where(p => p.GoodId == id).ToListAsync();
            foreach (var price in prices)
                model.PriceShops.Add(new ShopPriceViewModel
                {
                    idPrice = price.Id,
                    idShop = price.ShopId,
                    ShopName = price.Shop.Name,
                    Price = price.Price
                });
            foreach (var shop in await db.Shops.ToListAsync())
                if (model.PriceShops.Count(p => p.idShop == shop.Id) == 0)
                    model.PriceShops.Add(new ShopPriceViewModel
                    {
                        idShop = shop.Id,
                        ShopName = shop.Name,
                        Price = 0
                    });
            return View("Good", model);
        }

        public IActionResult Create()
        {
            var g = new GoodViewModel();
            foreach (var s in db.Shops.ToList())
                g.PriceShops.Add(new ShopPriceViewModel { idShop = s.Id, ShopName = s.Name });
            return View("Good", g);
        }

        [HttpPost]
        public async Task<IActionResult> Save(GoodViewModel g)
        {
            if (ModelState.IsValid)
            {
                if (g.Id == 0)
                {
                    var good = new Good { Name = g.Name, Article = g.Article, BarCode = g.BarCode, Unit = g.Unit, Price = g.Price };
                    db.Goods.Add(good);
                    foreach (var p in g.PriceShops)
                    {
                        var shop = await db.Shops.FirstOrDefaultAsync(s => s.Id == p.idShop);
                        var price = new GoodPrice { Good = good, Shop = shop, Price = p.Price };
                        await db.GoodPrices.AddAsync(price);
                        var goodBalance = new GoodBalance
                        {
                            Shop = shop,
                            Good = good,
                            Count = 0
                        };
                        db.GoodBalances.Add(goodBalance);
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                if (g.Id != 0)
                {
                    var good = await db.Goods.FirstOrDefaultAsync(good => good.Id == g.Id);
                    if (good != null)
                    {
                        good.Name = g.Name;
                        good.Article = g.Article;
                        good.BarCode = g.BarCode;
                        good.Unit = g.Unit;
                        good.Price = g.Price;
                        foreach (var p in g.PriceShops)
                        {
                            if (p.idPrice == 0)
                            {
                                var shop = await db.Shops.FirstOrDefaultAsync(s => s.Id == p.idShop);
                                var price = new GoodPrice { Good = good, Shop = shop, Price = p.Price };
                                await db.GoodPrices.AddAsync(price);
                            }
                            if (p.idPrice != 0)
                            {
                                var price = await db.GoodPrices.FirstOrDefaultAsync(price => price.Id == p.idPrice);
                                price.Price = p.Price;
                            }
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(g);
        }

        public async Task<IActionResult> Delete(int id, string find="")
        {
            var good = await db.Goods.Where(g => g.Id == id).FirstOrDefaultAsync();
            if (good != null)
            {
                db.Goods.Remove(good);
                await db.SaveChangesAsync();
            };
            return RedirectToAction("Search", new { find });
        }

        public async Task<IActionResult> PrintPriceStepShop()
        {
            var shops = await db.Shops.ToListAsync();
            var shopSelected = new List<ShopSelected>();
            foreach (var shop in shops)
                shopSelected.Add(new ShopSelected
                {
                    Id = shop.Id,
                    Name = shop.Name,
                    isSelected = false
                });
            return View(shopSelected);
        }

        [HttpPost]
        public async Task<IActionResult> PrintPriceStepGoods( List<ShopSelected> shops)
        {
            var idShops =new List<int>();
            foreach (var shop in shops)
                if(shop.isSelected)
                    idShops.Add(shop.Id);
            ViewData["idShops"] = idShops;
            return View(await db.Goods.Include(g => g.GoodPrices).ToListAsync());
        }

        static Dictionary<Guid, List<Good>> GoodSelectedListForPrint = new Dictionary<Guid, List<Good>>();
        [HttpPost]
        public async Task<IActionResult> PrintPriceStepSuccess([FromBody] Models.GoodPricePrintModel model)
        {
            var goods = await db.Goods.Include(g => g.GoodPrices.Where(p => model.Shops.Contains(p.ShopId))).Where(g=>model.Goods.Contains(g.Id)).ToListAsync();
            var query= db.Goods.Include(g => g.GoodPrices.Where(p => model.Shops.Contains(p.ShopId))).Where(g => model.Goods.Contains(g.Id));
            var str= query.ToQueryString();
            System.Diagnostics.Debug.WriteLine(str);
            Guid uuid = Guid.NewGuid();
            GoodSelectedListForPrint.Add(uuid, goods);
            return Ok(uuid);
        }

        public IActionResult PrintPriceStepPrint(Guid uuid, string typeprice="_Type1")
        {
            var goods = GoodSelectedListForPrint.FirstOrDefault(d => d.Key == uuid).Value;
            return View("PrintPriceStepPrint_"+ typeprice, goods);
        }
    }
}
