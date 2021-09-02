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
    public class WriteofController : Controller
    {
        shopContext db;
        public WriteofController(shopContext db)
        {
            this.db = db;
        }
        // GET: WriteofController
        public ActionResult Index()
        {
            return View();
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

        // GET: WriteofController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WriteofController/Edit/5
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

        // GET: WriteofController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WriteofController/Delete/5
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
