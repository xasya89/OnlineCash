using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using OnlineCash.Models.BuyerSynchronization;

namespace OnlineCash.Controllers.Api.OnlineCash
{
    [Route("api/onlinecash/[controller]")]
    [ApiController]
    public class BuyersController : ControllerBase
    {
        shopContext _db;
        public BuyersController(shopContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<BuyerSynchModel> result = new List<BuyerSynchModel>();
            var buyers = await _db.Buyers.Include(b => b.DiscountCard).OrderBy(b => b.Name).ToListAsync();
            foreach (var buyer in buyers)
                result.Add(new BuyerSynchModel
                {
                    Uuid = buyer.Uuid,
                    Name = buyer.Name,
                    Birthday = buyer.Birthday,
                    DiscountCardNum = buyer.DiscountCard.Num,
                    DiscountType = buyer.DiscountType,
                    SumBuy = buyer.SumBuy
                });
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] List<BuyerSynchModel> model)
        {
            var buyersDb = await _db.Buyers.Where(b => model.Any(m => m.Uuid == b.Uuid)).ToListAsync();
            foreach(var buyerdb in buyersDb)
                buyerdb.SumBuy= model.Where(m => m.Uuid == buyerdb.Uuid).FirstOrDefault().SumBuy;
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
