using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using Microsoft.AspNetCore.Authorization;

namespace OnlineCash.Controllers
{
    public class ArrivalsController : Controller
    {
        public shopContext db;
        public ILogger<ArrivalsController> logger;
        public IConfiguration configuration;
        public ArrivalsController(shopContext db, ILogger<ArrivalsController> logger, IConfiguration configuration)
        {
            this.db = db;
            this.logger = logger;
            this.configuration = configuration;
        }
        [Authorize]
        public async Task<IActionResult> Index() =>
            View(await db.Arrivals.Include(a => a.Supplier).Include(s=>s.Shop).OrderByDescending(a => a.DateArrival).ToListAsync());

        public async Task<IActionResult> Create(int ShopId=-1)
        {
            ViewData["ShopId"] = ShopId;
            ViewData["Action"] = "Create";
            ViewBag.Shops = await db.Shops.ToListAsync();
            ViewBag.Suppliers = await db.Suppliers.OrderBy(s=>s.Name).ToListAsync();
            return View("Edit", new Arrival { Id=-1});
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int ArrivalId=-1)
        {
            var model = await db.Arrivals.Include(a=>a.Supplier).Include(a=>a.ArrivalGoods).ThenInclude(a=>a.Good).Where(a => a.Id == ArrivalId).FirstOrDefaultAsync();
            if (model == null)
                return Redirect("index");
            ViewData["ShopId"] = model.ShopId;
            ViewData["Action"] = "Edit";
            ViewBag.Shops = await db.Shops.ToListAsync();
            ViewBag.Suppliers = await db.Suppliers.OrderBy(s => s.Name).ToListAsync();
            return View("Edit", model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Arrival model)
        {

            //var model = System.Text.Json.JsonSerializer.Deserialize<Arrival>(models.ToString());
            var shop = await db.Shops.Where(s => s.Id == model.ShopId).FirstOrDefaultAsync();
            var supplier = await db.Suppliers.Where(s => s.Id == model.SupplierId).FirstOrDefaultAsync();
            if (model.Num == "" || model.DateArrival == DateTime.MinValue || shop == null || supplier == null)
                return BadRequest();
            Arrival arrival = null;
            decimal priceAll = model.ArrivalGoods.Sum(g => g.Price);
            double countAll = model.ArrivalGoods.Sum(g => g.Count);
            if (model.Id == -1)
            {
                arrival = new Arrival
                {
                    Num = model.Num,
                    DateArrival = model.DateArrival,
                    Shop = shop,
                    Supplier = supplier,
                    PriceAll = priceAll,
                    CountAll = countAll,
                    isSuccess = model.isSuccess
                };
                db.Arrivals.Add(arrival);
            }
            else
            {
                arrival = await db.Arrivals.Where(a => a.Id == model.Id).FirstOrDefaultAsync();
                if (arrival == null)
                    return BadRequest();
                arrival.Num = model.Num;
                arrival.DateArrival = model.DateArrival;
                arrival.Shop = shop;
                arrival.Supplier = supplier;
                arrival.PriceAll = priceAll;
                arrival.CountAll = countAll;
                arrival.isSuccess = model.isSuccess;
                //Найдем удаленные товары
                foreach (var arrivalgood in await db.ArrivalGoods.Where(a => a.ArrivalId == model.Id).ToListAsync())
                    if (model.ArrivalGoods.Where(m => m.Id == arrivalgood.Id).FirstOrDefault() == null)
                        db.Remove(arrivalgood);
            };

            foreach (var modelGood in model.ArrivalGoods)
                if (modelGood.Id == -1)
                {
                    var good = await db.Goods.Where(g => g.Id == modelGood.GoodId).FirstOrDefaultAsync();
                    if (good == null)
                        return BadRequest();
                    var arrivalGood = new ArrivalGood
                    {
                        Arrival = arrival,
                        Good = good,
                        Price = modelGood.Price,
                        Count = modelGood.Count
                    };
                    db.ArrivalGoods.Add(arrivalGood);
                }
                else
                {
                    var arrivalGood = await db.ArrivalGoods.Where(a => a.Id == modelGood.Id).FirstOrDefaultAsync();
                    if (arrivalGood == null)
                        return BadRequest();
                    arrivalGood.Price = modelGood.Price;
                    arrivalGood.Count = modelGood.Count;
                };
            if (model.isSuccess)
                foreach(ArrivalGood modelGood in arrival.ArrivalGoods)
                {
                    var goodBalance = await db.GoodBalances.Where(g => g.GoodId == modelGood.GoodId & g.ShopId==model.ShopId).FirstOrDefaultAsync();
                    if (goodBalance == null)
                    {
                        var good = await db.Goods.Where(g => g.Id == modelGood.GoodId).FirstOrDefaultAsync();
                        db.GoodBalances.Add(new GoodBalance
                        {
                            Shop = shop,
                            Good = good,
                            Count = modelGood.Count
                        });
                    }
                    else
                        goodBalance.Count += modelGood.Count;
                }    
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
