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
        NotificationOfEventInSystemService _notificationService;
        //ILogger _logger;
        public ArrivalSynchController(ArrivalService service/*, ILogger logger*/, NotificationOfEventInSystemService notificationService)
        {
            this.service = service;
            _notificationService = notificationService;
        }

        [HttpPost("{shopId}")]
        public async Task<IActionResult> Save(int shopId, [FromBody] ArrivalSynchModel model)
        {
            HttpContext.Response.Headers.Add("interceptro-x", "interceptor");
            try
            {
                var arrival=await service.SaveSynchAsync(shopId, model);
                await _notificationService.Send($"Приходная накладная {arrival.Num} на сумму {arrival.SumArrival}", "Arrivals/Edit?ArrivalId="+arrival.Id);
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
