using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Services;
using OnlineCash.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineCash.Controllers
{
    [Authorize(Roles = "admin")]
    public class ReportsController : Controller
    {
        shopContext db;
        IReportsService reportsService;
        IGoodBalanceService goodBalanceService;
        MoneyBalanceService _moneyBalanceService;
        GoodCountAnalyseService _analyseService;
        GoodCountBalanceService _countBalanceService;
        public ReportsController(shopContext db, 
            IReportsService reportsService, 
            IGoodBalanceService goodBalanceService, 
            MoneyBalanceService moneyBalanceService, 
            GoodCountAnalyseService analyseService,
            GoodCountBalanceService countBalanceService)
        {
            this.db = db;
            this.reportsService = reportsService;
            this.goodBalanceService = goodBalanceService;
            _moneyBalanceService = moneyBalanceService;
            _analyseService = analyseService;
            _countBalanceService = countBalanceService;
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
        public async Task<IActionResult> GoodCountBalanceInit()
        {
            await _countBalanceService.Init();
            return Redirect("GoodBalanceHistory");
        }

        public async Task<IActionResult> GoodCountBalance(string find, int goodGroupId, bool viewNull)
        {
            var goodGroups = new List<DataBaseModels.GoodGroup>() { new DataBaseModels.GoodGroup { } };
            goodGroups.AddRange(await db.GoodGroups.ToListAsync());
            ViewBag.GoodGroups = goodGroups;
            ViewBag.ViewNull = viewNull;
            return View(await db.GoodCountBalanceCurrents.Include(c => c.Good)
                .Where(c =>(goodGroupId==0 || c.Good.GoodGroupId==goodGroupId) & (!viewNull || c.Count>0) & EF.Functions.Like(c.Good.Name, $"%{find}%"))
                .OrderBy(c => c.Good.Name).ToListAsync());
        }
        public async Task<List<dynamic>> GoodCountDetail(int goodId)
        {
            DateTime dateLast = DateTime.Now.AddMonths(-1).Date;
            decimal? countLast = (await db.GoodCountBalances
                .Where(b => b.GoodId == goodId & DateTime.Compare(b.Period.Date, dateLast) == 0)
                .FirstOrDefaultAsync())?.Count;
            var stocktackingLast = await db.Stocktakings
                .Include(s=>s.StocktakingSummaryGoods).Where(s=>s.Status==DataBaseModels.DocumentStatus.Confirm)
                .OrderBy(s=>s.Create).LastAsync();
            if(stocktackingLast!=null)
            {
                countLast = stocktackingLast.StocktakingSummaryGoods.Where(s => s.GoodId == goodId).FirstOrDefault()?.CountFact ?? countLast;
                dateLast = stocktackingLast.Create;
            }
            List<dynamic> histoies = new List<dynamic>() { new { Title="Начало", Date=dateLast, DateStr=dateLast.ToString("dd.MM.yy"), Count=countLast } };
            foreach (var doc in await db.GoodCountDocHistories.Where(d => d.Date > dateLast).ToListAsync())
                histoies.Add(new { Title = doc.TypeDoc.GetDescription(), Date = doc.Date, DateStr = doc.Date.ToString("dd.MM.yy"), Count = doc.Count });
            return histoies;
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
