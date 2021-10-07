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

        [HttpPost("openshift/{idShop}")]
        public async Task<IActionResult> OpenShift(int idShop)
        {
            if (!await service.OpenShift(idShop))
                return BadRequest();
            return Ok();
        }

        [HttpPost("buy/{idShop}")]
        public async Task<IActionResult> Buy(int idShop,[FromBody] List<CashBoxBuyReturnModel> buylist)
        {
            if (!await service.Buy(idShop, buylist))
                return BadRequest();
            return Ok();
        }

        [HttpPost("closeshift/{idShop}")]
        public async Task<IActionResult> CloseShift(int idShop)
        {
            if (!await service.CloseShift(idShop))
                return BadRequest();
            return Ok();
        }
    }
}
