using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using RMK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RMK.Services;

namespace RMK.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAtolService kkt;

        public HomeController(ILogger<HomeController> logger, IAtolService kkt)
        {
            _logger = logger;
            this.kkt = kkt;
        }

        public IActionResult Index()
        {
            /*
            kkt.ToggleSession();
            kkt.OpenShift("Кассир Иванов И.", "123456789047");
            */
            return View();
        }

        public IActionResult Privacy()
        {
            //kkt.OpenShift("Никульшин ","312301001");

            return Ok(new DataBase.Class1().Persons);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
