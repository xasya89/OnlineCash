using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMK.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace RMK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodBarcodesController : ControllerBase
    {
        ILogger<GoodBarcodesController> logger;
        onlinecashContext db;
        public GoodBarcodesController(ILogger<GoodBarcodesController> logger, onlinecashContext db)
        {
            this.logger = logger;
            this.db = db;
        }

        [HttpGet("{barcode}")]
        public async Task<IActionResult> GetGood(string barcode)
        {
            var good = await db.GoodPrices.Include(g => g.Good).Include(s => s.Shop).Where(p => p.Good.BarCode == barcode & p.ShopId == 1).FirstOrDefaultAsync();
            if (good != null)
                return Ok(new GoodPriceInShop
                {
                    Id = good.GoodId,

                    Name = good.Good.Name,
                    Price = good.Good.Price,
                    Article = good.Good.Article,
                    BarCode = good.Good.BarCode
                });
            else
                return BadRequest();
        }
    }
}
