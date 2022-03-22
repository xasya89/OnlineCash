using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OnlineCash.Services;

namespace OnlineCash.Controllers
{
    public class RevaluationController : Controller
    {
        RevaluationService _service;
        NotificationOfEventInSystemService _notificationService;
        public RevaluationController(RevaluationService service, NotificationOfEventInSystemService notificationService)
        {
            _service = service;
            _notificationService = notificationService;
        }
        public async Task<IActionResult> Index() => View(await _service.GetRevaluations());
        public async Task<IActionResult> Edit(int id) => View(await _service.GetRevaluation(id));
    }
}
