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
        shopContext db;
        IGoodBalanceService goodBalance;
        public WriteofController(shopContext db, IGoodBalanceService goodBalance)
        {
            this.db = db;
            this.goodBalance = goodBalance;
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
            var shop = await db.Shops.Where(s => s.Id == model.ShopId).FirstOrDefaultAsync();
            if(model.Id==0)
            {
                decimal sumAll = model.WriteofGoods.Sum(w => (decimal)w.Count * w.Price);
                var writeof = new Writeof
                {
                    Status=model.IsSuccess ? DocumentStatus.Confirm : DocumentStatus.New,
                    Shop = shop,
                    DateWriteof = model.DateWriteof,
                    IsSuccess=model.IsSuccess,
                    Note = model.Note,
                    SumAll = sumAll
                };
                db.Writeofs.Add(writeof);
                foreach(var wgood in model.WriteofGoods)
                {
                    var good = await db.Goods.Where(g => g.Id == wgood.GoodId).FirstOrDefaultAsync();
                    var writeofGood = new WriteofGood
                    {
                        Writeof=writeof,
                        Good = good,
                        Count = wgood.Count,
                        Price = wgood.Price
                    };
                    db.WriteofGoods.Add(writeofGood);
                }
                await db.SaveChangesAsync();
                if (writeof.IsSuccess)
                    await goodBalance.CalcAsync(writeof.ShopId, writeof.DateWriteof);

                return Ok();
            }
            return BadRequest();
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
            try
            {
                var writeof = await db.Writeofs.Where(w => w.Id == model.Id).Include(w => w.WriteofGoods).FirstOrDefaultAsync();
                if (writeof.Status == DocumentStatus.Confirm)
                    return BadRequest("Акт уже проведен");
                if (writeof.Status == DocumentStatus.Remove)
                    return BadRequest("Акт уже отменен");
                if (writeof == null)
                    return BadRequest("Акт не найден");
                var docStatusOld = writeof.Status;
                writeof.Status = model.IsSuccess ? DocumentStatus.Confirm : DocumentStatus.Edit;
                writeof.ShopId = model.ShopId;
                writeof.DateWriteof = model.DateWriteof;
                writeof.IsSuccess = model.IsSuccess;
                writeof.Note = model.Note;
                writeof.SumAll = model.WriteofGoods.Sum(wg => (decimal)wg.Count * wg.Price);
                await db.SaveChangesAsync();
                //Удалим удаленные позиции
                foreach (var writegood in writeof.WriteofGoods)
                    if (model.WriteofGoods.Where(wg => wg.Id == writegood.Id).FirstOrDefault() == null)
                        db.WriteofGoods.Remove(writegood);
                //Добавим новые позиции
                foreach (var wgood in model.WriteofGoods.Where(wg => wg.Id == -1).ToList())
                    db.WriteofGoods.Add(new WriteofGood
                    {
                        Writeof=writeof,
                        GoodId = wgood.GoodId,
                        Count = wgood.Count,
                        Price = wgood.Price
                    });
                //Изменим существующие позиции
                foreach(var wgood in model.WriteofGoods.Where(wg=>wg.Id!=-1).ToList())
                {
                    var writegood = await db.WriteofGoods.Where(wg => wg.Id == wgood.Id).FirstOrDefaultAsync();
                    writegood.Count = wgood.Count;
                    writegood.Price = wgood.Price;
                };
                await db.SaveChangesAsync();
                if (docStatusOld!=DocumentStatus.Confirm & writeof.Status == DocumentStatus.Confirm)
                    await goodBalance.CalcAsync(writeof.ShopId, writeof.DateWriteof);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task GoodBalanceMinus(Writeof model)
        {
            var goodGroups = from WriteofGood in model.WriteofGoods group WriteofGood by WriteofGood.GoodId;
            List<GoodBalanceModel> goodBalances = new List<GoodBalanceModel>();
            foreach (IGrouping<int, WriteofGood> group in goodGroups)
            {
                goodBalances.Add(new GoodBalanceModel
                {
                    GoodId = group.Key,
                    ShopId = model.ShopId,
                    Count = group.Sum(gr => gr.Count)
                });
            }
            await goodBalance.MinusAsync(goodBalances);
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
