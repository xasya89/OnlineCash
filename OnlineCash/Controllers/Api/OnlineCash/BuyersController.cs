using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using DatabaseBuyer;

namespace OnlineCash.Controllers.Api.OnlineCash
{
    [Route("api/onlinecash/[controller]")]
    [ApiController]
    public class BuyersController : ControllerBase
    {
        shopbuyerContext _dbBuyer;
        public BuyersController( shopbuyerContext dbBuyer) => _dbBuyer = dbBuyer;

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _dbBuyer.Buyers.ToListAsync());
    }
}
