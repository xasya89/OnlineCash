using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models;
using OnlineCash.Services;
using Serilog;

namespace OnlineCash.Controllers.Api.OnlineCash
{
    [Route("api/onlinecash/[controller]")]
    [ApiController]
    public class ArrivalSynchController : ControllerBase
    {
        ArrivalService service;
        //ILogger _logger;
        public ArrivalSynchController(ArrivalService service/*, ILogger logger*/)
        {
            this.service = service;
        }

        [HttpPost("{shopId}")]
        public async Task<IActionResult> Save(int shopId, [FromBody] ArrivalSynchModel model)
        {
            try
            {
                await service.SaveSynchAsync(shopId, model);
                return Ok();
            }
            catch(Exception ex)
            {
                //_logger.Error($"{ex.Message} \n {ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
