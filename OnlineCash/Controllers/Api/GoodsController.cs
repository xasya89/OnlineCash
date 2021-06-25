using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataBase;

namespace OnlineCash.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        public shopContext db;
        public GoodsController( shopContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string Name)
            => Ok( await db.Goods.Where(g => EF.Functions.Like(g.Name, $"%{Name}%")).ToListAsync());

        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetBarcode(string barcode)
        {

            return Ok( JsonSerializer.Serialize(
                await db.Goods.Include(g => g.GoodPrices).Where(g => g.BarCode == barcode).FirstOrDefaultAsync()
                ));

        }
    }
}
