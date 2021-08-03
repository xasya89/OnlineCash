using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineCash.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarcodesController : ControllerBase
    {
        shopContext _db;
        public BarcodesController(shopContext db)
        {
            _db = db;
        }
        [HttpGet("{barcode}")]
        public async Task<IActionResult> GetBarcode(string barcode)
        {
            return Ok(JsonSerializer.Serialize(
                await _db.Goods.Include(g => g.GoodPrices).Include(g => g.Supplier).Where(g => g.BarCode == barcode).FirstOrDefaultAsync()
                ));
        }
    }
}
