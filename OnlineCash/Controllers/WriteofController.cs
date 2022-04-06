using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using OnlineCash.Services;
using OnlineCash.Models;

namespace OnlineCash.Controllers
{
    public class WriteofController : Controller
    {
        private readonly shopContext db;
        private readonly IGoodBalanceService goodBalance;
        private readonly IWriteofService _writeofService;
        public WriteofController(shopContext db, IGoodBalanceService goodBalance, IWriteofService writeofService)
        {
            this.db = db;
            this.goodBalance = goodBalance;
            _writeofService = writeofService;
        }
        // GET: WriteofController
        public async Task<ActionResult> Index()
        {
            var writeofs = await db.Writeofs.Include(w=>w.Shop).OrderBy(w => w.DateWriteof).ToListAsync();
            return View(writeofs);
        }

        // GET: WriteofController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WriteofController/Create
        public ActionResult Create()
        {
            ViewBag.Shops = db.Shops.OrderBy(s => s.Name).ToList();
            return View("One",new Writeof { DateWriteof = DateTime.Now }) ;
        }

        // POST: WriteofController/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Writeof model)
        {
            await _writeofService.Create(model);
            return Ok();
        }

        // GET: WriteofController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var writeof = await db.Writeofs
                .Include(w => w.Shop)
                .Include(w => w.WriteofGoods).ThenInclude(wg => wg.Good)
                .Where(w => w.Id == id)
                .FirstOrDefaultAsync();
            ViewBag.Shops = await db.Shops.OrderBy(s => s.Name).ToListAsync();
            if (writeof == null)
                return RedirectToAction(nameof(Index));
            return View("One",writeof);
        }

        // POST: WriteofController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, [FromBody] Writeof model)
        {
            await _writeofService.Edit(model);
            return Ok();
        }

        // GET: WriteofController/Delete/5
        public async Task<IActionResult> Canceled(int id)
        {
            var writeof = await db.Writeofs.Where(w => w.Id == id).FirstOrDefaultAsync();
            if(writeof is null)
                return BadRequest("Акт не найден");
            if (writeof.Status == DocumentStatus.Confirm)
                return BadRequest("Нельзя удалить подтвержденный акт");
            writeof.Status = DocumentStatus.Remove;
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
