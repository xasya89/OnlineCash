using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Models;
using System.Text.Json;
using OnlineCash.Models.GoodPrint;

namespace OnlineCash.Controllers
{
    public class GoodsPrintController : Controller
    {
        shopContext db;
        public GoodsPrintController(shopContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> SelectionGoods()
        {
            ViewBag.Shops = await db.Shops.OrderBy(s => s.Name).ToListAsync();
            return View();
        }

        /// <summary>
        /// Печать ценника для продавца
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GenerateTamplateCashier([FromQuery] bool noPrice)
        {
            List<int> idShops = JsonSerializer.Deserialize<List<int>>(Request.Query["idShops"]);
            List<int> idGoods = JsonSerializer.Deserialize<List<int>>(Request.Query["idGoods"]);
            var goodBalances = new List<GoodWithGoodBalanceModel>();
            //var goods = await db.Goods.Where(g => idGoods.Contains(g.Id)).Include(g => g.GoodPrices.Where(p=>idShops.Contains(p.ShopId))).ThenInclude(p=>p.Shop).ToListAsync();
            var goods = await db.Goods.Where(g => idGoods.Contains(g.Id)).Include(g => g.BarCodes).Include(g => g.GoodPrices.Where(p => idShops.Contains(p.ShopId))).ThenInclude(p => p.Shop).ToListAsync();
            foreach (var good in goods)
                goodBalances.Add(new GoodWithGoodBalanceModel
                {
                    Good = good
                });
            ViewBag.NoPrice = noPrice;
            return View(goodBalances);
        }

        /// <summary>
        /// Печать ценника на витрине
        /// </summary>
        public async Task<IActionResult> GenerateTamplateShowCase([FromQuery] bool noPrice)
        {
            List<int> idShops = JsonSerializer.Deserialize<List<int>>(Request.Query["idShops"]);
            List<int> idGoods = JsonSerializer.Deserialize<List<int>>(Request.Query["idGoods"]);
            var goodBalances = new List<GoodWithGoodBalanceModel>();
            var goods = await db.Goods.Where(g => idGoods.Contains(g.Id)).Include(g=>g.BarCodes).Include(g => g.GoodPrices.Where(p => idShops.Contains(p.ShopId))).ThenInclude(p=>p.Shop).ToListAsync();
            foreach (var good in goods)
                goodBalances.Add(new GoodWithGoodBalanceModel
                {
                    Good = good
                });
            ViewBag.NoPrice = noPrice;
            return View(goodBalances);
        }

        /// <summary>
        /// Печать ценника на витрине за 100г
        /// </summary>
        public async Task<IActionResult> GenerateTamplateShowCase100([FromQuery] bool noPrice)
        {
            List<int> idShops = JsonSerializer.Deserialize<List<int>>(Request.Query["idShops"]);
            List<int> idGoods = JsonSerializer.Deserialize<List<int>>(Request.Query["idGoods"]);
            var goodBalances = new List<GoodWithGoodBalanceModel>();
            var goods = await db.Goods.Where(g => idGoods.Contains(g.Id)).Include(g => g.BarCodes).Include(g => g.GoodPrices.Where(p => idShops.Contains(p.ShopId))).ThenInclude(p => p.Shop).ToListAsync();
            foreach (var good in goods)
                goodBalances.Add(new GoodWithGoodBalanceModel
                {
                    Good = good
                });
            ViewBag.NoPrice = noPrice;
            return View(goodBalances);
        }
    }
}
