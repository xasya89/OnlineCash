using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models;
using OnlineCash.Services;
using OnlineCash.Filters;

namespace OnlineCash.Controllers.Api.OnlineCash
{
    [Route("api/onlinecash/[controller]")]
    [ApiController]
    [TypeFilterAttribute(typeof(ControlDocSynchFilter))]
    [TypeFilterAttribute(typeof(ControlExceptionDocSynchFilter))]
    public class RevaluationSynchController : ControllerBase
    {
        private readonly RevaluationService _service;
        NotificationOfEventInSystemService _notificationService;
        public RevaluationSynchController(RevaluationService service, NotificationOfEventInSystemService notificationService)
        {
            _service = service;
            _notificationService = notificationService;
        }

        [HttpPost("{shopId}")]
        public async Task<IActionResult> GetSynch([FromHeader(Name = "doc-uuid")] Guid? uuidSynch, int shopId, [FromBody] RevaluationModel model)
        {
            await _service.SaveSynch(model, uuidSynch);
            return Ok();
        }
    }
}
