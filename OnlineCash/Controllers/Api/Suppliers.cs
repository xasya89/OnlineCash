using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class Suppliers : ControllerBase
    {
        shopContext db;
        ILogger<Suppliers> logger;
        public Suppliers(shopContext db, ILogger<Suppliers> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<List<Supplier>> Get() => await db.Suppliers.ToListAsync();
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Supplier model)
        {
            if (model.Name == "" || model.Inn == "")
                return BadRequest();
            var supplierdb = await db.Suppliers.Where(s => s.Name == model.Name & s.Inn == model.Inn).FirstOrDefaultAsync();
            if (supplierdb != null)
                return BadRequest();
            db.Suppliers.Add(model);
            await db.SaveChangesAsync();
            return Ok(model);
        }
    }
}
