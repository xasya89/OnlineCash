using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Services;

namespace OnlineCash.Controllers
{
    public class ReportsController : Controller
    {
        shopContext db;
        IReportsService reportsService;
        public ReportsController(shopContext db, IReportsService reportsService)
        {
            this.db = db;
            this.reportsService = reportsService;
        }

        [HttpGet]
        public async Task<IActionResult> GoodBalanceHistory()
        {
            ViewBag.Shops = await db.Shops.OrderBy(s => s.Name).ToListAsync();
            ViewBag.GoodGroups = await db.GoodGroups.ToListAsync();
            return View(await reportsService.GetGoodBalanceHistory(DateTime.Now, db.Shops.FirstOrDefault().Id));
        }
    }
}
