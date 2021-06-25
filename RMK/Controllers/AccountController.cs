using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMK.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace RMK.Controllers
{
    public class AccountController : Controller
    {
        ILogger<AccountController> logger;
        onlinecashContext db;
        public AccountController(ILogger<AccountController> logger, onlinecashContext db)
        {
            this.logger = logger;
            this.db = db;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var cashier = await db.Cashiers.FirstOrDefaultAsync(c => c.PinCode == model.PinCode);
                if(cashier!=null)
                {
                    await Authenticate(cashier.Id);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        private async Task Authenticate(int cashierId)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, cashierId.ToString())
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
