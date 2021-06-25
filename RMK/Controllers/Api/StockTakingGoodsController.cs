using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMK.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTakingGoodsController : ControllerBase
    {
        onlinecashContext db;
        public StockTakingGoodsController(onlinecashContext db)
        {
            this.db = db;
        }

        [HttpGet("{idStocktaking}")]
        public async Task<IActionResult> Get(int idSotcktaking) =>
           Ok(await db.StocktakingGoods.Where(g => g.StocktakingId == idSotcktaking).ToListAsync());
    }
}
