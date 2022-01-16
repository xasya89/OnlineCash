using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Services;
using OnlineCash.Models;

namespace OnlineCash.Controllers
{
    public class ReportsController : Controller
    {
        shopContext db;
        IReportsService reportsService;
        IGoodBalanceService goodBalanceService;
        public ReportsController(shopContext db, IReportsService reportsService, IGoodBalanceService goodBalanceService)
        {
            this.db = db;
            this.reportsService = reportsService;
            this.goodBalanceService = goodBalanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GoodBalanceHistory()
        {
            ViewBag.Shops = await db.Shops.OrderBy(s => s.Name).ToListAsync();
            ViewBag.GoodGroups = await db.GoodGroups.OrderBy(gr=>gr.Name).ToListAsync();
            return View(await reportsService.GetGoodBalanceHistory(DateTime.Now, db.Shops.FirstOrDefault().Id, null));
        }

        [HttpPost]
        public async Task<IActionResult> GoodBalanceHistory(ReportGoodBalanceModel model)
        {
            ViewBag.Shops = await db.Shops.OrderBy(s => s.Name).ToListAsync();
            ViewBag.GoodGroups = await db.GoodGroups.OrderBy(gr=>gr.Name).ToListAsync();
            return View(await reportsService.GetGoodBalanceHistory(model.CurDate, model.ShopId, model.GoodGroupId));
        }

        [HttpGet]
        public async Task<IActionResult> GoodBalanceCalc(int shopId, string day)
        {
            var daycalc = Convert.ToDateTime(day);
            await goodBalanceService.CalcAsync(shopId, daycalc);
            return RedirectToAction(nameof(GoodBalanceHistory));
        }

        [HttpGet]
        public async Task<IActionResult> MoneyBalanceHistory(int shopId)
            => View(await db.MoneyBalanceHistories.OrderByDescending(m => m.DateBalance).ToListAsync());
    }
}
