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
        MoneyBalanceService _moneyBalanceService;
        GoodCountAnalyseService _analyseService;
        public ReportsController(shopContext db, IReportsService reportsService, IGoodBalanceService goodBalanceService, MoneyBalanceService moneyBalanceService, GoodCountAnalyseService analyseService)
        {
            this.db = db;
            this.reportsService = reportsService;
            this.goodBalanceService = goodBalanceService;
            _moneyBalanceService = moneyBalanceService;
            _analyseService = analyseService;
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

        [HttpGet]
        public async Task<IActionResult> CalcMoneyBalanceHistory(int shopId, string withStr)
        {
            DateTime with;
            if (!DateTime.TryParse(withStr, out with))
                return BadRequest();
            await _moneyBalanceService.Calculate(shopId, with);
            return View("MoneyBalanceHistory", await db.MoneyBalanceHistories.OrderByDescending(m => m.DateBalance).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GoodCountAnalyse(int shopId, string start, string stop)
        {
            DateTime startDay = DateTime.Parse(start);
            DateTime stopDay = DateTime.Parse(stop);
            ViewBag.Start = startDay;
            ViewBag.Stop = stopDay;
            return View(await _analyseService.GetAnalyse(shopId, startDay, stopDay));
        }
    }
}
