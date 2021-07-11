using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineCash.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodGroupsController : ControllerBase
    {
        ILogger<GoodGroupsController> logger;
        IConfiguration configuration;
        shopContext db;
        public GoodGroupsController(shopContext db, ILogger<GoodGroupsController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.db = db;
        }

        [HttpGet]
        public async Task<IEnumerable<GoodGroup>> Get() => await db.GoodGroups.Include(g=>g.Goods).ThenInclude(g=>g.Supplier).OrderBy(g=>g.Name).ToListAsync();

        // GET api/<GoodGroupsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GoodGroup model)
        {
            db.GoodGroups.Add(model);
            await db.SaveChangesAsync();
            return Ok(model);
        }

        // PUT api/<GoodGroupsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] GoodGroup model)
        {
        }

        // DELETE api/<GoodGroupsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await db.Goods.CountAsync(g => g.GoodGroupId == id) != 0)
                return BadRequest();
            var goodgroup = await db.GoodGroups.Where(gg => gg.Id == id).FirstOrDefaultAsync();
            if (goodgroup==null)
                return BadRequest();
            db.GoodGroups.Remove(goodgroup);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
