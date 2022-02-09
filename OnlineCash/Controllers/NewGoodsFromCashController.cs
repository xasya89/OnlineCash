using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineCash.Controllers
{
    public class NewGoodsFromCashController : Controller
    {
        shopContext _db;
        public NewGoodsFromCashController(shopContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
            => View(await _db.NewGoodFromCashes.Include(n => n.Good).Where(n=>n.Processed==false).ToListAsync());

        public async Task<IActionResult> Open(int id)
        {
            var newGood= await _db.NewGoodFromCashes.Where(n => n.Id == id).FirstOrDefaultAsync();
            newGood.Processed = true;
            await _db.SaveChangesAsync();
            return Redirect("/goods/details/" + newGood.GoodId);
        }
    }
}
