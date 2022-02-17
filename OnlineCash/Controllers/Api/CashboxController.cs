using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Services;
using OnlineCash.Models;
using OnlineCash.Models.CashBox;

namespace OnlineCash.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashboxController : ControllerBase
    {
        ICashBoxService service;
        NotificationOfEventInSystemService _notification;
        public CashboxController(ICashBoxService service, NotificationOfEventInSystemService notification)
        {
            this.service = service;
            _notification = notification;
        }

        [HttpPost("openshift/{idShop}/{uuid}/{start}")]
        public async Task<IActionResult> OpenShift(int idShop, Guid uuid,string start)
        {
            if (!await service.OpenShift(idShop,uuid,Convert.ToDateTime(start)))
                return BadRequest();
            return Ok();
        }

        [HttpPost("buy/{idShop}")]
        public async Task<IActionResult> Buy(Guid uuid,[FromBody] List<CashBoxBuyReturnModel> buylist)
        {
            if (!await service.Buy(uuid, buylist))
                return BadRequest();
            return Ok();
        }

        [HttpPost("sell/{uuidShift}")]
        public async Task<IActionResult> Sell(Guid uuidShift, [FromBody] CashBoxCheckSellModel check)
        {
            await service.Sell(uuidShift, check);
            return Ok();
        }

        [HttpPost("closeshift/{uuid}/{stop}")]
        public async Task<IActionResult> CloseShift(Guid uuid, string stop)
        {
            var shift = await service.CloseShift(uuid, Convert.ToDateTime(stop));
            if (shift==null)
                return BadRequest();
            await _notification.Send(@$"Закрыта смена 
Наличные: {shift.SumNoElectron}
Безналичные: {shift.SumElectron}
Возвраты наличные: {shift.SumReturnCash}
Возвраты безналичные: {shift.SumReturnElectron}
", "ReportsSells/Details/"+shift.Id);
            return Ok();
        }
    }
}
