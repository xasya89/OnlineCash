using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Services;
using OnlineCash.Models;

namespace OnlineCash.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashboxController : ControllerBase
    {
        ICashBoxService service;
        public CashboxController(ICashBoxService service)
        {
            this.service = service;
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

        [HttpPost("closeshift/{uuid}/{stop}")]
        public async Task<IActionResult> CloseShift(Guid uuid, string stop)
        {
            if (!await service.CloseShift(uuid, Convert.ToDateTime(stop)))
                return BadRequest();
            return Ok();
        }
    }
}
