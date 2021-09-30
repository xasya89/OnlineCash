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
using OnlineCash.Services;

namespace OnlineCash.Controllers
{
    public class ArrivalsController : Controller
    {
        public shopContext db;
        public ILogger<ArrivalsController> logger;
        public IConfiguration configuration;
        IGoodBalanceService goodBalanceService;
        public ArrivalsController(shopContext db, ILogger<ArrivalsController> logger, IConfiguration configuration, IGoodBalanceService goodBalanceService)
        {
            this.db = db;
            this.logger = logger;
            this.configuration = configuration;
            this.goodBalanceService = goodBalanceService;
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
            ViewBag.BankAccounts = await db.BankAccounts.OrderBy(b => b.Alias).ToListAsync();
            return View("Edit", new Arrival { Id=-1, DateArrival=DateTime.Now});
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int ArrivalId=-1)
        {
            var model = await db.Arrivals.Include(a=>a.Supplier).Include(a=>a.ArrivalGoods).ThenInclude(a=>a.Good).Include(a => a.ArrivalPayments).Where(a => a.Id == ArrivalId).FirstOrDefaultAsync();
            if (model == null)
                return Redirect("index");
            ViewData["ShopId"] = model.ShopId;
            ViewData["Action"] = "Edit";
            ViewBag.Shops = await db.Shops.ToListAsync();
            ViewBag.Suppliers = await db.Suppliers.OrderBy(s => s.Name).ToListAsync();
            ViewBag.BankAccounts = await db.BankAccounts.OrderBy(b => b.Alias).ToListAsync();
            return View("Edit", model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Arrival model)
        {
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
                        PriceSell=modelGood.PriceSell,
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
                    arrivalGood.PriceSell = modelGood.PriceSell;
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
                    //Изменим цену на товар
                    var goodPrice = await db.GoodPrices.Where(p => p.ShopId == arrival.ShopId & p.GoodId == modelGood.GoodId).FirstOrDefaultAsync();
                    goodPrice.Price = modelGood.PriceSell;
                }
            //Платежи
            var bankaccouns = await db.BankAccounts.ToListAsync();
            foreach(var modelpayment in model.ArrivalPayments)
                if(modelpayment.Id==0)
                {
                    var payment = new ArrivalPayment
                    {
                        Arrival = arrival,
                        BankAccount = bankaccouns.Where(b => b.Id == modelpayment.BankAccountId).FirstOrDefault(),
                        DatePayment=modelpayment.DatePayment,
                        Sum=modelpayment.Sum
                    };
                    db.ArrivalPayments.Add(payment);
                }
            else
                {
                    var payment = await db.ArrivalPayments.Where(p => p.Id == modelpayment.Id).FirstOrDefaultAsync();
                    payment.BankAccountId = modelpayment.BankAccountId;
                    payment.DatePayment = modelpayment.DatePayment;
                    payment.Sum = modelpayment.Sum;
                }
            //Подсчет итогов
            arrival.SumArrivals = model.ArrivalGoods.Sum(a => (decimal) a.Count * a.Price);
            arrival.SumPayments = model.ArrivalPayments.Sum(p => p.Sum);
            await db.SaveChangesAsync();
            await goodBalanceService.CalcAsync(shop.Id, model.DateArrival);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ReportProblem()
        {
            List<Models.ArrivalReport> result = new List<Models.ArrivalReport>();
            var suppliers = await db.Suppliers.ToListAsync();
            //var supplierGroups = db.Arrivals.Include(a=>a.Supplier).GroupBy(a => a.SupplierId);
            var supplierGroups =await db.Arrivals.GroupBy(a => a.SupplierId).Select(a => new { SupplierId = a.Key, SumArrival = a.Sum(a => a.SumArrivals), SumPayments = a.Sum(a => a.SumPayments) }).ToListAsync();
            foreach (var supplier in supplierGroups)
                result.Add(new Models.ArrivalReport
                {
                    Supplier = suppliers.Where(s => s.Id == supplier.SupplierId).FirstOrDefault(),
                    SumPayments = supplier.SumPayments,
                    SumArrivals = supplier.SumArrival
                });
            return View(result.OrderBy(r=>r.Supplier.Name).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> PaymentsList()
        {
            ViewBag.Banks = await db.BankAccounts.ToListAsync();
            return View(await db.Arrivals.Where(a => a.SumArrivals - a.SumPayments > 0).Include(a => a.Shop).OrderBy(a=>a.DateArrival).OrderBy(a=>a.Shop.Name).ToListAsync());
        }
            
        [HttpPost]
        public async Task<IActionResult> PaymentsList([FromBody]Models.PaymentListSaveModel model)
        {
            if (model.Sum == 0 || model.ArrivalId == 0 || model.BankId == 0)
                return BadRequest();
            var payment = new ArrivalPayment { ArrivalId = model.ArrivalId, BankAccountId = model.BankId, DatePayment = DateTime.Now, Sum = model.Sum };
            db.ArrivalPayments.Add(payment);
            var arrival = await db.Arrivals.Where(a => a.Id == model.ArrivalId).FirstOrDefaultAsync();
            arrival.SumPayments = arrival.SumPayments + model.Sum;
            await db.SaveChangesAsync();
            return Ok(model);
        }
    }
}
