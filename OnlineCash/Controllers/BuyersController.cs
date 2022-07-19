using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DatabaseBuyer;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Controllers
{
    public class BuyersController : Controller
    {
        public shopbuyerContext _dbBuyer;
        public shopContext _db;
        public BuyersController(shopbuyerContext dbBuyer, shopContext db)
        {
            _dbBuyer = dbBuyer;
            _db = db;
        }
        public async Task<IActionResult> Index() =>
            View(await _dbBuyer.Buyers.OrderByDescending(b => b.SumBuy).ToListAsync());

        /*
        public async Task<IActionResult> GetBuyer(int id)
        {

        }
        */
    }
}
