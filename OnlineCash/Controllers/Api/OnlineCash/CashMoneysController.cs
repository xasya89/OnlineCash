using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;
using OnlineCash.Services;
using Microsoft.Extensions.Logging;

namespace OnlineCash.Controllers.Api.OnlineCash
{
    [Route("api/onlinecash/[controller]")]
    [ApiController]
    public class CashMoneysController : ControllerBase
    {

        CashMoneyService _moneyService;
        ILogger<CashMoneysController> _logger;
        NotificationOfEventInSystemService _notification;
        public CashMoneysController(CashMoneyService moneyService, ILogger<CashMoneysController> logger, NotificationOfEventInSystemService notification)
        {
            _moneyService = moneyService;
            _logger = logger;
            _notification = notification;
        }

        [HttpPost("{shopId}")]
        public async Task<IActionResult> Recive(int shopId, [FromBody]CashMoney model)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Ошибка валидации");
                var cashMoney=await _moneyService.Add(shopId, model);
                await _notification.Send($"{cashMoney.TypeOperation.GetDescription()} на сумму {cashMoney.Sum}. Примечание {cashMoney.Note}", "CashMoney");
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                return BadRequest();
            }
        }
    }
}
