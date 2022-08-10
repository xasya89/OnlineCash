using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models;
using OnlineCash.Services;
using Serilog;
using OnlineCash.Filters;

namespace OnlineCash.Controllers.Api.OnlineCash
{
    [Route("api/onlinecash/[controller]")]
    [ApiController]
    [TypeFilterAttribute(typeof(ControlDocSynchFilter))]
    [TypeFilterAttribute(typeof(ControlExceptionDocSynchFilter))]
    public class ArrivalSynchController : ControllerBase
    {
        ArrivalService service;
        //ILogger _logger;
        public ArrivalSynchController(ArrivalService service)
        {
            this.service = service;
        }

        [HttpPost("{shopId}")]
        public async Task<IActionResult> Save([FromHeader(Name = "doc-uuid")] Guid uuid, int shopId, [FromBody] ArrivalSynchModel model)
        {
            await service.SaveSynchAsync(shopId, model, uuid);
            return Ok();
        }
    }
}
