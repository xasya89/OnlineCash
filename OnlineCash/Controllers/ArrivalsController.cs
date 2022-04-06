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
        ArrivalService _arrivalService;
        public ArrivalsController(shopContext db, 
            ILogger<ArrivalsController> logger, 
            IConfiguration configuration, 
            IGoodBalanceService goodBalanceService,
            ArrivalService arrivalService
            )
        {
            this.db = db;
            this.logger = logger;
            this.configuration = configuration;
            this.goodBalanceService = goodBalanceService;
            _arrivalService = arrivalService;
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
            if (model.Id == -1)
                await _arrivalService.Create(model);
            else
                await _arrivalService.Edit(model);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ReportProblem()
        {
            List<Models.ArrivalReport> result = new List<Models.ArrivalReport>();
            var suppliers = await db.Suppliers.ToListAsync();
            var supplierGroups =await db.Arrivals.GroupBy(a => a.SupplierId).Select(a => new { SupplierId = a.Key, SumArrival = a.Sum(a => a.SumArrival), SumPayments = a.Sum(a => a.SumPayments) }).ToListAsync();
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
            var with = DateTime.Now.AddDays(-30).Date;
            var by = DateTime.Now.Date;
            ViewBag.With = with;
            ViewBag.By = by;
            return View(await db.Arrivals
                .Include(a => a.Shop)
                .Include(a => a.Supplier)
                .Include(a => a.ArrivalPayments)
                .Where(a => DateTime.Compare(with,a.DateArrival)==-1 & DateTime.Compare(a.DateArrival,by)==-1)
                .OrderByDescending(a => a.DateArrival)
                .ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PaymentsList([Bind]string with, [Bind] string by)
        {
            ViewBag.Banks = await db.BankAccounts.ToListAsync();
            if(!string.IsNullOrEmpty(with) & string.IsNullOrEmpty(by))
            {
                var withDate = Convert.ToDateTime(with).Date;
                ViewBag.With = withDate;
                return View(await db.Arrivals
                    .Include(a => a.Shop)
                    .Include(a => a.Supplier)
                    .Include(a => a.ArrivalPayments)
                    .Where(a => DateTime.Compare(a.DateArrival, withDate) >= 0)
                    .OrderByDescending(a => a.DateArrival)
                    .ToListAsync());
            };
                
            if (!string.IsNullOrEmpty(by) & string.IsNullOrEmpty(with))
            {
                DateTime byDate = Convert.ToDateTime(by).Date;
                ViewBag.By = byDate;
                return View(await db.Arrivals
                    .Include(a => a.Shop)
                    .Include(a => a.Supplier)
                    .Include(a => a.ArrivalPayments)
                    .Where(a => a.isSuccess & DateTime.Compare(a.DateArrival, byDate) <= 0)
                    .OrderByDescending(a => a.DateArrival)
                    .ToListAsync());
            }

            if (!string.IsNullOrEmpty(with) & !string.IsNullOrEmpty(by))
            {
                DateTime withDate = Convert.ToDateTime(with).Date;
                DateTime byDate = Convert.ToDateTime(by).Date;
                ViewBag.With = withDate;
                ViewBag.By = byDate;
                return View(await db.Arrivals
                    .Include(a => a.Shop)
                    .Include(a => a.Supplier)
                    .Include(a => a.ArrivalPayments)
                    .Where(a => a.isSuccess == true & DateTime.Compare(a.DateArrival, Convert.ToDateTime(with)) >= 0 & DateTime.Compare(a.DateArrival, Convert.ToDateTime(by)) <= 0)
                    .OrderByDescending(a => a.DateArrival)
                    .ToListAsync());
            }
            var withDt = DateTime.Now.AddDays(-30).Date;
            var byDt = DateTime.Now.Date;
            ViewBag.With = withDt;
            ViewBag.By = byDt;
            return View(await db.Arrivals
                .Include(a => a.Shop)
                .Include(a=>a.Supplier)
                .Include(a=>a.ArrivalPayments)
                .Where(a => DateTime.Compare(withDt, a.DateArrival) == -1 & DateTime.Compare(a.DateArrival, byDt) == -1)
                .OrderByDescending(a => a.DateArrival)
                .ToListAsync());
        }

        [HttpPut]
        public async Task<IActionResult> PaymentsList([FromBody]Models.PaymentListSaveModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
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
