using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineCash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsSynchController : ControllerBase
    {
        public IConfiguration configuration;
        public ILogger<GoodsSynchController> logger;
        public shopContext db;
        public GoodsSynchController(IConfiguration configuration, ILogger<GoodsSynchController> logger, shopContext db)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.db = db;

            var goods = db.Goods.Where(g => g.Uuid.ToString() == "00000000-0000-0000-0000-000000000000").ToList();
            foreach (var g in goods)
                g.Uuid = Guid.NewGuid();
            db.SaveChanges();
        }
        [HttpGet("{idShop:int}")]
        public async Task<IActionResult> Get(int idShop)
            => Ok(JsonSerializer.Serialize(await db.Goods.Include(g => g.GoodPrices.Where(p => p.ShopId == idShop)).ToListAsync(),new JsonSerializerOptions {ReferenceHandler=ReferenceHandler.Preserve }));
    }
}
