using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;
using OnlineCash.Services;

namespace OnlineCash.Controllers
{
    public class CashMoneyController : Controller
    {
        shopContext _db;
        ILogger<CashMoneyController> _Logger;
        CashMoneyService _moneyService;
        public CashMoneyController(shopContext db, ILogger<CashMoneyController> logger, CashMoneyService moneyService)
        {
            _db = db;
            _Logger = logger;
            _moneyService = moneyService;
        }
        public async Task<IActionResult> Index() => View(await _moneyService.Get());
    }
}
