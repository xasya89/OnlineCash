using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMK.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RMK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAtolService kkt;
        private readonly onlinecashContext db;
        private readonly int idShop;
        public ShiftController(ILogger<HomeController> logger, IConfiguration configuration, IAtolService kkt, onlinecashContext db)
        {
            _logger = logger;
            this.kkt = kkt;
            this.db = db;
            idShop = Convert.ToInt32(configuration.GetSection("ShopId").Value);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var shiftOld = await db.Shifts.FirstOrDefaultAsync(s => s.Stop == null);
            string shiftStatus = shiftOld == null ? "close" : "open";
            switch (shiftStatus)
            {
                case "close":
                    var cashier = await db.Cashiers.FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(User.Identity.Name==null ? 2 : User.Identity.Name));
                    var shop = await db.Shops.FirstOrDefaultAsync(s => s.Id == idShop);
                    db.Shifts.Add(new DataBase.Shift
                    {
                        Cashier = cashier,
                        Shop = shop
                    });
                    db.SaveChanges();
                    kkt.OpenShift(cashier.Name,cashier.Inn);
                    break;
                case "open":
                    var shift =await db.Shifts.FirstOrDefaultAsync(s => s.Stop == null);
                    shift.Stop = DateTime.Now;
                    db.SaveChanges();
                    kkt.CloseShift();
                    break;
            }
            return Ok();
        }
    }
}
