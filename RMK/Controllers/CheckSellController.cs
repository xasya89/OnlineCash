using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMK.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RMK.ViewModels;
using DataBase;

namespace RMK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckSellController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAtolService kkt;
        private readonly onlinecashContext db;

        public CheckSellController(ILogger<HomeController> logger, IAtolService kkt, onlinecashContext db)
        {
            _logger = logger;
            this.kkt = kkt;
            this.db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Sell([FromBody] CheckSellViewModel model)
        {
            var cashier = await db.Cashiers.FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(User.Identity.Name==null ? 2 : User.Identity.Name));
            if(cashier==null)
                cashier = await db.Cashiers.FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(User.Identity.Name == null ? 1 : User.Identity.Name));
            model.CashierName = cashier.Name;
            model.CashierInn = cashier.Inn;
            var goods = await db.Goods.ToListAsync();
            foreach(var good in model.Goods)
                foreach(var g in goods)
                    if (good.Id == g.Id)
                    {
                        good.Name = g.Name;
                        break;
                    };
            if (kkt.CheckSell(model))
            {
                var shift = await db.Shifts.FirstOrDefaultAsync(s => s.Stop == null);
                var check = new CheckSell
                {
                    Shift=shift,
                    IsElectron = model.isElectron,
                    Sum = model.Sum,
                    SumDiscont = model.SumDiscont,
                    SumAll = model.SumAll
                };
                db.CheckSells.Add(check);
                foreach(var good in model.Goods)
                {
                    var checkGood = new CheckGood
                    {
                        CheckSell = check,
                        Good = await db.Goods.FirstOrDefaultAsync(g => g.Id == good.Id),
                        Cost = good.Price,
                        Count = good.Count
                    };
                    db.CheckGoods.Add(checkGood);
                };
                db.SaveChanges();
            }
            return Ok(model);
        }
    }
}
