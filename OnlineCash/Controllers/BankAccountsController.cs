using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Controllers
{
    
    public class BankAccountsController : Controller
    {
        shopContext db;
        public BankAccountsController(shopContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.BankAccounts.OrderBy(b=>b.Alias).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create() => View("Edit", new BankAccount());
        [HttpGet]
        public async Task<IActionResult> Edit(int id) => View("Edit", await db.BankAccounts.Where(b => b.Id == id).FirstOrDefaultAsync());

        [HttpPost]
        public async Task<IActionResult> Save(BankAccount model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);
            if (model.Id == 0)
                db.BankAccounts.Add(model);
            else
            {
                var account = await db.BankAccounts.Where(b => b.Id == model.Id).FirstOrDefaultAsync();
                if (account == null)
                    return View("Edit", model);
                account.Alias = model.Alias;
                account.Num = model.Num;
                account.BankName = model.BankName;
                account.KorShet = model.KorShet;
                account.Bik = model.Bik;
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var account = await db.BankAccounts.Where(b => b.Id == id).FirstOrDefaultAsync();
            var arrivalPayments = await db.ArrivalPayments.Where(p => p.BankAccountId == id).ToListAsync();
            if (account == null && arrivalPayments.Count>0)
                return RedirectToAction("Index");
            db.BankAccounts.Remove(account);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
