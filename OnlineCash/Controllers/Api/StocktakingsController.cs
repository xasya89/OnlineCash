using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocktakingsController : ControllerBase
    {
        public IConfiguration configuration;
        public ILogger<StocktakingsController> logger;
        public shopContext db;
        public StocktakingsController(IConfiguration configuration, ILogger<StocktakingsController> logger, shopContext db)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Set([FromBody] Models.StocktakingModel model)
        {
            var shop = await db.Shops.Where(s => s.Id == model.ShopId).FirstOrDefaultAsync();
            var stocktaking = new Stocktaking
            {
                Num = model.Num,
                Create = model.Create,
                Shop = shop
            };
            db.Stocktakings.Add(stocktaking);
            var goods = await db.Goods.ToListAsync();
            foreach(var stgood in model.StocktakingGoods)
            {
                var good = goods.Where(g => g.Uuid == stgood.GoodUuid).FirstOrDefault();
                var stgoodDb = new StocktakingGood
                {
                    Stocktaking = stocktaking,
                    Good = good,
                    Count = stgood.Count,
                    CountFact=0
                };
                db.StocktakingGoods.Add(stgoodDb);
            }
            await db.SaveChangesAsync();
            return Ok("success");
        }
    }
}
