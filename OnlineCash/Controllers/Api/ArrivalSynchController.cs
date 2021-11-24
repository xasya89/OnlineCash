using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models;
using OnlineCash.Services;

namespace OnlineCash.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArrivalSynchController : ControllerBase
    {
        ArrivalService service;
        public ArrivalSynchController(ArrivalService service)
        {
            this.service = service;
        }

        [HttpPost("{shopId}")]
        public async Task<IActionResult> Save(int shopId, [FromBody] ArrivalSynchModel model)
        {
            if (await service.SaveSynchAsync(shopId, model))
                return Ok();
            else
                return BadRequest();
        }
    }
}
