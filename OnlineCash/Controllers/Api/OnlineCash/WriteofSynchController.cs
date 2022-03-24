using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models;
using OnlineCash.Services;
using OnlineCash.Models;
using OnlineCash.Filters;

namespace OnlineCash.Controllers.Api
{
    [Route("api/onlinecash/[controller]")]
    [ApiController]
    [TypeFilterAttribute(typeof(ControlDocSynchFilter))]
    [TypeFilterAttribute(typeof(ControlExceptionDocSynchFilter))]
    public class WriteofSynchController : ControllerBase
    {
        IWriteofService writeofService;
        NotificationOfEventInSystemService _notificationService;
        public WriteofSynchController(IWriteofService writeofService, NotificationOfEventInSystemService notificationService)
        {
            this.writeofService = writeofService;
            _notificationService = notificationService;
        }

        [HttpPost("{shopId}")]
        public async Task<IActionResult> Post(int shopId, [FromBody] WriteofSynchModel model)
        {
            var writeOf= await writeofService.SaveSynch(shopId, model);
            await _notificationService.Send($"Списание на сумму {writeOf.SumAll} Примечание {writeOf.Note}", "Writeof/edit/" + writeOf.Id);
            return Ok();
        }
    }
}
