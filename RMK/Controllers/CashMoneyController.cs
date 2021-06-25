using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMK.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashMoneyController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAtolService kkt;

        public CashMoneyController(ILogger<HomeController> logger, IAtolService kkt)
        {
            _logger = logger;
            this.kkt = kkt;
        }

        [HttpGet("{id}/{sum}")]
        public IActionResult Get(string id, decimal sum)
        {
            switch (id.ToLower())
            {
                case "income":
                    kkt.CashIncome(sum);
                    break;
                case "outcome":
                    kkt.CashOutcome(sum);
                    break;
            }
            return Ok();
        }
    }
}
