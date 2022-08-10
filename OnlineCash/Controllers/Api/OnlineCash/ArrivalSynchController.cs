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
        public async Task<IActionResult> Save([FromHeader(Name = "doc-uuid")] Guid uuid, int shopId, [FromBody] ArrivalSynchModel model)
        {
            /*
            string uuidStr = HttpContext.Request.Headers.Where(h => h.Key.ToLower() == "doc-uuid").Select(h => h.Value).FirstOrDefault();
            Guid? uuidSynch = null;
            if (uuidStr != null)
                uuidSynch = Guid.Parse(uuidStr);
            */
            await service.SaveSynchAsync(shopId, model, uuid);

            return Ok();
        }
    }
}
