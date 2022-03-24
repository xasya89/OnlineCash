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
        NotificationOfEventInSystemService _notificationService;
        GoodCountBalanceService _countBalanceService;
        //ILogger _logger;
        public ArrivalSynchController(ArrivalService service/*, ILogger logger*/, 
            NotificationOfEventInSystemService notificationService,
            GoodCountBalanceService countBalanceService)
        {
            this.service = service;
            _notificationService = notificationService;
            _countBalanceService = countBalanceService;
        }

        [HttpPost("{shopId}")]
        public async Task<IActionResult> Save(int shopId, [FromBody] ArrivalSynchModel model)
        {
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
