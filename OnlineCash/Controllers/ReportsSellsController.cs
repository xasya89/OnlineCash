using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Models;

namespace OnlineCash.Controllers
{
    public class ReportsSellsController : Controller
    {
        ILogger<ReportsSellsController> logger;
        shopContext db;
        IConfiguration configuration;
        public ReportsSellsController(ILogger<ReportsSellsController> logger, shopContext db, IConfiguration configuration)
        {
            this.logger = logger;
            this.db = db;
            this.configuration = configuration;
        }
        // GET: ReportsSellsController
        public async Task<IActionResult> Index() =>
            View(await db.Shifts.Include(s => s.Shop).OrderByDescending(s => s.Start).ToListAsync());

        // GET: ReportsSellsController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var shift = await db.Shifts
                .Include(s => s.Shop)
                .Include(s => s.ShiftSales)
                .ThenInclude(c => c.Good)
                .Where(s => s.Id == id).FirstOrDefaultAsync();
            var reportSell = new ReportSellModel { 
                Shop = shift.Shop, 
                Start = shift.Start,
                SumSell=shift.SumSell,
            };
            foreach (var sell in shift.ShiftSales)
                reportSell.ReportGoods.Add(new ReportSellGoodModel
                {
                    Good = sell.Good,
                    CountSell = sell.Count,
                    CountReturn = sell.CountReturn,
                    CountAll = (decimal)sell.Count - sell.CountReturn,
                    SumAll = sell.Sum - sell.SumReturn
                });
            return View("Details", reportSell);
        }

        // GET: ReportsSellsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportsSellsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReportsSellsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReportsSellsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReportsSellsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReportsSellsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
