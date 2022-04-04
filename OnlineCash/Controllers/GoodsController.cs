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
using OnlineCash.Services;
using OnlineCash.ViewModels.GoodsPrint;
using OnlineCash.Services;

namespace OnlineCash.Controllers
{
    [Authorize]
    public class GoodsController : Controller
    {
        public IConfiguration configuration;
        public ILogger<GoodsController> logger;
        public shopContext db;
        private readonly GoodCountBalanceService _balanceService;
        private readonly GoodService _goodService;
        public GoodsController(
            IConfiguration configuration, 
            ILogger<GoodsController> logger, 
            shopContext db, 
            GoodCountBalanceService balanceService,
            GoodService goodService)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.db = db;
            _balanceService = balanceService;
            _goodService = goodService;
        }

        public async Task<IActionResult> Index()
        {
            string find= HttpContext.Session.GetString("SearchGoods");
            ViewBag.Find = find;
            int searchGroup = 0;
            int.TryParse(HttpContext.Session.GetString("SearchGoodsGroup"), out searchGroup);
            bool filterIsDeleted = false;
            bool.TryParse(HttpContext.Session.GetString("SearchGoodsIsDeleted"), out filterIsDeleted);
            ViewBag.FindGroup = searchGroup;
            ViewBag.FindIsDeleted = filterIsDeleted;
            ViewBag.GoodGroups = await db.GoodGroups.ToListAsync();
            ViewBag.Shops = await db.Shops.ToListAsync();
            ViewBag.Suppliers = await db.Suppliers.ToListAsync();
            return View(await db.Goods
                .Where(g=> EF.Functions.Like(g.Name, $"%{find}%"))
                .Where(g=>searchGroup==0 || g.GoodGroupId==searchGroup)
                .Where(g=>filterIsDeleted || g.IsDeleted==false)
                .Include(g => g.GoodPrices)
                .ThenInclude(gp => gp.Shop)
                .Include(g => g.GoodGroup)
                .Include(g => g.BarCodes)
                .OrderBy(g => g.Name)
                .ToListAsync()
                );
        }

        public async Task<IActionResult> Search(string find, int? group)
        {
            HttpContext.Session.SetString("SearchGoods", find ?? "");
            string searchGroup = group is null ? "" : group.ToString();
            HttpContext.Session.SetString("SearchGoodsGroup", searchGroup );
            find = HttpContext.Session.GetString("SearchGoods");
            bool filterIsDeleted = false;
            bool.TryParse(HttpContext.Session.GetString("SearchGoodsIsDeleted"), out filterIsDeleted);
            ViewBag.Find = find;
            ViewBag.FindGroup = group;
            ViewBag.FindIsDeleted = filterIsDeleted;
            ViewBag.GoodGroups = await db.GoodGroups.ToListAsync();
            ViewBag.Shops = await db.Shops.OrderBy(s => s.Name).ToListAsync();
            if(find!="")
            {
                var barcode = await db.BarCodes.Where(b => b.Code == find).FirstOrDefaultAsync();
                if (barcode != null)
                    return View("Index", await db.Goods.Where(g => g.Id == barcode.GoodId).ToListAsync());
            }

            return View("Index", await db.Goods
                .Where(g => EF.Functions.Like(g.Name, $"%{find}%"))
                .Where(g=>group==null || g.GoodGroupId==group)
                .Where(g=>filterIsDeleted || g.IsDeleted==false)
                .Include(g=>g.GoodPrices)
                .ThenInclude(gp=>gp.Shop)
                .Include(g=>g.GoodGroup)
                .Include(g=>g.BarCodes)
                .OrderBy(g => g.Name)
                .ToListAsync()
                );
        }

        public IActionResult SetSearch(string search="")
        {
            HttpContext.Session.SetString("SearchGoods", Convert.ToString(search));
            return Ok();
        }

        public IActionResult SetFilterIsDeleted()
        {
            bool filterIsDeleted = false;
            bool.TryParse(HttpContext.Session.GetString("SearchGoodsIsDeleted"), out filterIsDeleted);
            filterIsDeleted = !filterIsDeleted;
            HttpContext.Session.SetString("SearchGoodsIsDeleted", Convert.ToString(filterIsDeleted));
            return RedirectToAction("Index");
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

        [HttpGet]
        public async Task<IActionResult> PrintPriceAll(int idShop)
            => View("PrintPriceStepPrint_TypePriceNoCount", await db.GoodGroups.Include(g => g.Goods).ThenInclude(g => g.GoodPrices.Where(p => p.ShopId == idShop)).OrderBy(gr=>gr.Name).ToListAsync());
        /*
        {
            var groups = await db.GoodGroups.Include(g => g.Goods).ThenInclude(g => g.GoodPrices.Where(p => p.ShopId == idShop)).ToListAsync();
            
            var goods = await db.Goods.Include(g => g.GoodPrices.Where(p => p.ShopId==idShop)).OrderBy(g=>g.Name).ToListAsync();
            List<Models.GoodWithGoodBalanceModel> goodWithBalances = new List<Models.GoodWithGoodBalanceModel>();
            var goodBalances = await db.GoodBalances.Where(gb => gb.ShopId==idShop).ToListAsync();
            foreach (var good in goods)
            {
                goodWithBalances.Add(new Models.GoodWithGoodBalanceModel
                {
                    Good = good,
                    CountOnBalance = goodBalances.Where(gb => gb.GoodId == good.Id).FirstOrDefault()?.Count
                });
            }
            return View("PrintPriceStepPrint_TypePriceNoCount", goodWithBalances);

        }
        */
        public async Task<IActionResult> Details(int id)
        {
            /*
            var enums=Enum.GetValues(typeof(SpecialTypes)).Cast<SpecialTypes>().ToList();
            foreach(var enum1 in enums) {};
            */
            var good = await db.Goods.Include(g=>g.BarCodes).Where(g=>g.Id==id).FirstOrDefaultAsync(g => g.Id == id);
            var model = new GoodViewModel();
            model.Id = good.Id;
            model.Name = good.Name;
            model.GoodGroupId = good.GoodGroupId;
            model.Article = good.Article;
            model.BarCodes = good.BarCodes;
            model.BarCode = good.BarCode;
            model.Unit = good.Unit;
            model.SpecialType = good.SpecialType;
            model.VPackage = good.VPackage;
            model.Price = good.Price;
            model.SupplierId = good.SupplierId;
            var prices = await db.GoodPrices.Include(p => p.Shop).Where(p => p.GoodId == id).ToListAsync();
            foreach (var price in prices)
                model.PriceShops.Add(new ShopPriceViewModel
                {
                    idPrice = price.Id,
                    idShop = price.ShopId,
                    ShopName = price.Shop.Name,
                    Price = price.Price,
                    BuySuccess=price.BuySuccess
                });
            foreach (var shop in await db.Shops.ToListAsync())
                if (model.PriceShops.Count(p => p.idShop == shop.Id) == 0)
                    model.PriceShops.Add(new ShopPriceViewModel
                    {
                        idShop = shop.Id,
                        ShopName = shop.Name,
                        Price = 0
                    });
            ViewBag.Goods = await db.Goods.ToListAsync();
            ViewBag.Groups = await db.GoodGroups.OrderBy(gr=>gr.Name).ToListAsync();
            ViewBag.Suppliers = await db.Suppliers.OrderBy(s => s.Name).ToListAsync();
            return View("Good", model);
        }

        public async Task<IActionResult> Create()
        {
            var g = new GoodViewModel();
            foreach (var s in db.Shops.ToList())
                g.PriceShops.Add(new ShopPriceViewModel { idShop = s.Id, ShopName = s.Name });
            g.GoodGroupId = (await db.GoodGroups.FirstOrDefaultAsync()).Id;
            ViewBag.Goods = await db.Goods.ToListAsync();
            ViewBag.Groups = await db.GoodGroups.OrderBy(gr => gr.Name).ToListAsync();
            ViewBag.Suppliers = await db.Suppliers.OrderBy(s => s.Name).ToListAsync();
            return View("Good", g);
        }

        [HttpPost]
        public async Task<IActionResult> Save(GoodViewModel g)
        {
            if (ModelState.IsValid)
            {
                var goodGroup = await db.GoodGroups.Where(gr => gr.Id == g.GoodGroupId).FirstOrDefaultAsync();
                Supplier supplier = null;
                supplier = await db.Suppliers.Where(s => s.Id == g.SupplierId).FirstOrDefaultAsync();
                g.GoodGroup = goodGroup;
                g.Supplier = supplier;
                if (g.GoodPrices == null)
                    g.GoodPrices = new List<GoodPrice>();
                foreach (var p in g.PriceShops)
                    g.GoodPrices.Add(new GoodPrice { Id=p.idPrice, ShopId = p.idShop, Price = p.Price, BuySuccess=p.BuySuccess });
                if (g.Id == 0)
                {
                    g.Uuid = Guid.NewGuid();
                    var newGood=await _goodService.Add(g as Good);
                };
                if (g.Id != 0)
                {
                    /*
                    foreach (var p in g.PriceShops)
                        g.GoodPrices.Add(new GoodPrice { Id = p.idPrice, ShopId = p.idShop, Price = p.Price, BuySuccess = p.BuySuccess });
                    */
                    await _goodService.Edit(g as Good);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Goods = await db.Goods.ToListAsync();
            ViewBag.Groups = await db.GoodGroups.OrderBy(gr => gr.Name).ToListAsync();
            ViewBag.Suppliers = await db.Suppliers.OrderBy(s => s.Name).ToListAsync();
            return View("Good",g);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var good = await db.Goods.Where(g => g.Id == id).FirstOrDefaultAsync();
            if (good != null)
            {
                good.IsDeleted = true;
                //db.Goods.Remove(good);
                await db.SaveChangesAsync();
            };
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Recove(int id)
        {
            var good = await db.Goods.Where(g => g.Id == id).FirstOrDefaultAsync();
            if (good != null)
            {
                good.IsDeleted = false;
                //db.Goods.Remove(good);
                await db.SaveChangesAsync();
            };
            return RedirectToAction(nameof(Index));
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
            return View(await db.Goods.Where(g=>!g.IsDeleted).Include(g => g.GoodPrices).ToListAsync());
        }

        static Dictionary<Guid, List<Models.GoodWithGoodBalanceModel>> GoodSelectedListForPrint = new Dictionary<Guid, List<Models.GoodWithGoodBalanceModel>>();
        [HttpPost]
        public async Task<IActionResult> PrintPriceStepSuccess([FromBody] Models.GoodPricePrintModel model)
        {
            var goods = await db.Goods.Include(g => g.GoodPrices.Where(p => model.Shops.Contains(p.ShopId))).Where(g=>model.Goods.Contains(g.Id)).ToListAsync();
            List<Models.GoodWithGoodBalanceModel> goodWithBalances = new List<Models.GoodWithGoodBalanceModel>();
            var goodBalances = await db.GoodBalances.Where(gb => model.Shops.Contains(gb.ShopId)).ToListAsync();
            foreach (var good in goods)
            {
                goodWithBalances.Add(new Models.GoodWithGoodBalanceModel { 
                    Good=good,
                    CountOnBalance= goodBalances.Where(gb => gb.GoodId == good.Id).FirstOrDefault()?.Count
                });
            }
            Guid uuid = Guid.NewGuid();
            GoodSelectedListForPrint.Add(uuid, goodWithBalances);
            return Ok(uuid);
        }

        public IActionResult PrintPriceStepPrint(Guid uuid, string typeprice="_Type1")
        {
            var goods = GoodSelectedListForPrint.FirstOrDefault(d => d.Key == uuid).Value;
            return View("PrintPriceStepPrint_"+ typeprice, goods);
        }

        /// <summary>
        /// Печать ценников с выбранными группами и магазинами
        /// </summary>
        /// <param name="model"></param>
        public async Task<IActionResult> PrintPriceTagWithSelectionGroup([FromBody] Models.GoodPrint.SelectionGroupShopModel model) =>
            View(await db.GoodGroups.Where(gr => model.idGroups.Contains(gr.Id))
                .Include(gr => gr.Goods.Where(g=>g.IsDeleted==false).ToList())
                .ThenInclude(g => g.GoodPrices.Where(p => model.idShops.Contains(p.ShopId)))
                .ToListAsync());

    }
}
