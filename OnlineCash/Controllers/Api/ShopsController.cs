using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class ShopsController : ControllerBase
    {
        shopContext db;
        public ShopsController(shopContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<List<Shop>> Get() => await db.Shops.ToListAsync();
    }
}
