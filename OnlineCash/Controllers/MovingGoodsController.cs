using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Controllers
{
    public class MovingGoodsController : Controller
    {
        shopContext db;
        public MovingGoodsController(shopContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index()
            => View(await db.MoveDocs.Include(m => m.ConsigneeShop).Include(m => m.ConsignerShop).OrderBy(m => m.DateMove).ToListAsync());

        // GET: MovingGoodsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MovingGoodsController/Create
        public ActionResult Create()
        {
            ViewBag.Shops = db.Shops.ToList();
            return View("One",new MoveDoc());
        }

        // POST: MovingGoodsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MovingGoodsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MovingGoodsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MovingGoodsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MovingGoodsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
