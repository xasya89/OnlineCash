using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models;
using OnlineCash.Services;
using OnlineCash.Models;

namespace OnlineCash.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class WriteofSynchController : ControllerBase
    {
        IWriteofService writeofService;
        public WriteofSynchController(IWriteofService writeofService)
        {
            this.writeofService = writeofService;
        }

        [HttpPost("{shopId}")]
        public async Task<IActionResult> Post(int shopId, [FromBody] WriteofSynchModel model)
        {
            if (await writeofService.SaveSynch(shopId, model))
                return Ok();
            else
                return BadRequest();
        }
    }
}
