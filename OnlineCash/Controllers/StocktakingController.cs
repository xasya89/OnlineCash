using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using Microsoft.AspNetCore.Authorization;

namespace OnlineCash.Controllers
{
    [Authorize]
    public class StocktakingController : Controller
    {
        shopContext db;
        ILogger<StocktakingController> logger;
        public StocktakingController(shopContext db, ILogger<StocktakingController> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Shops = await db.Shops.ToListAsync();
            return View("Index",await db.Stocktakings.Include(s => s.Shop).OrderBy(s => s.Create).OrderByDescending(s=>s.Create).ToListAsync());
        }

        public async Task<IActionResult> Create(int idShop)
        {
            ViewBag.Goods = await db.Goods.Include(g => g.GoodPrices).ToListAsync();
            var shop = await db.Shops.Where(s => s.Id == idShop).FirstOrDefaultAsync();
            if (shop == null)
                return RedirectToAction("Index");
            int num = 1;
            List<Stocktaking> stocktakingold = await db.Stocktakings.Where(s => s.ShopId == idShop).ToListAsync();
            if (stocktakingold.Count > 0)
                num = stocktakingold.Max(s => s.Num) + 1;
            var stocktaking = new Stocktaking { Create = DateTime.Now, Num = (int)num, Shop = shop };
            db.Stocktakings.Add(stocktaking);
            var group = new StockTakingGroup { Stocktaking = stocktaking, Name = "Без группы" };
            db.StockTakingGroups.Add(group);
            await db.SaveChangesAsync();
            return View("Edit", stocktaking);
        }

        public async Task<IActionResult> Edit(int id) 
        {
            ViewBag.Goods = await db.Goods.Include(g => g.GoodPrices).ToListAsync();
            var stocktaking = await db.Stocktakings.Include(s => s.Shop).Include(s => s.StockTakingGroups).ThenInclude(gr=>gr.StocktakingGoods).ThenInclude(sg=>sg.Good).Where(s=>s.Id==id).FirstOrDefaultAsync();
            return View(stocktaking);
        }
        
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Models.StockTakingSaveModel model)
        {
            Stocktaking stocktaking = await db.Stocktakings.Include(s => s.StockTakingGroups).ThenInclude(gr=>gr.StocktakingGoods).Where(s => s.Id == model.id).FirstOrDefaultAsync();
            bool isSuccessOld = stocktaking.isSuccess;
            stocktaking.isSuccess = model.isSuccess;
            stocktaking.Status = model.isSuccess ? DocumentStatus.Confirm : stocktaking.Status;
            foreach(var sgr in model.Groups)
            {
                StockTakingGroup group = null;
                if(sgr.Id==-1)
                {
                    group = new StockTakingGroup
                    {
                        Name = sgr.Name,
                        Stocktaking = stocktaking
                    };
                    db.StockTakingGroups.Add(group);
                }
                else
                {
                    group = await db.StockTakingGroups.Where(gr => gr.Id == sgr.Id).FirstOrDefaultAsync();
                    if (group == null)
                        return BadRequest();
                    group.Name = sgr.Name;
                };
                //Добавим новые товары
                foreach (var sg in sgr.Goods.Where(g => g.id == -1).ToList())
                {
                    var good = await db.Goods.Include(g => g.GoodPrices).Where(g => g.Id == sg.idGood).FirstOrDefaultAsync();
                    StocktakingGood stocktakingGood = new StocktakingGood
                    {
                        StockTakingGroup = group,
                        Good = good,
                        Count = 0,
                        CountDB = 0,
                        CountFact = sg.CountFact,
                        Price = good.GoodPrices.Where(p => p.ShopId == stocktaking.ShopId).FirstOrDefault().Price
                    };
                    db.StocktakingGoods.Add(stocktakingGood);
                }
                //Изменим количество
                foreach (var sg in sgr.Goods.Where(g => g.id != -1).ToList())
                {
                    StocktakingGood stocktakingGood = await db.StocktakingGoods.Where(g => g.Id == sg.id).FirstOrDefaultAsync();
                    stocktakingGood.CountFact = sg.CountFact;
                }
            }
            await db.SaveChangesAsync();
            //Изменим количесво в GoodBalance
            if (model.isSuccess & !isSuccessOld)
            {
                stocktaking = await db.Stocktakings.Where(s => s.Id == model.id).Include(s => s.StockTakingGroups).ThenInclude(gr => gr.StocktakingGoods).FirstOrDefaultAsync();

                //Проверим количество в системе для товаров добавленных в инверторизацию
                var goodBalances = await db.GoodBalances.Include(b=>b.Good).ThenInclude(g=>g.GoodPrices.Where(p=>p.ShopId==stocktaking.ShopId)).Where(gb => gb.ShopId == stocktaking.ShopId).ToListAsync();
                foreach(var group in stocktaking.StockTakingGroups)
                    foreach(var good in group.StocktakingGoods)
                    {
                        var goodbalance = goodBalances.Where(b => b.GoodId == good.GoodId).FirstOrDefault();
                        if (goodbalance != null)
                        {
                            good.CountDB = goodbalance.Count;
                            goodbalance.Count = good.CountFact;
                        }
                        else //Если товара нет в goodblalnce, тогда его добавим
                            db.GoodBalances.Add(new GoodBalance
                            {
                                ShopId = stocktaking.ShopId,
                                GoodId = good.GoodId,
                                Count = good.CountFact
                            });
                    };
                //проверим наличие не учетнных товаров и создадим группу
                bool flagNoInvertoryGoods = false;
                foreach(var b in goodBalances.Where(b=>b.Count!=0).ToList())
                {
                    bool flag = false;
                    foreach (var group in stocktaking.StockTakingGroups)
                        foreach (var good in group.StocktakingGoods)
                            if (b.GoodId == good.GoodId)
                                flag = true;
                    flagNoInvertoryGoods = flag==false ? true : flagNoInvertoryGoods;
                };
                if (flagNoInvertoryGoods)
                {

                    var groupNoInvertory = new StockTakingGroup { Stocktaking = stocktaking, Name = "Не учтено" };
                    db.StockTakingGroups.Add(groupNoInvertory);
                    foreach (var b in goodBalances.Where(b=>b.Count!=0))
                    {
                        bool flag = false;
                        foreach (var group in stocktaking.StockTakingGroups)
                            foreach (var good in group.StocktakingGoods)
                                if (b.GoodId == good.GoodId)
                                    flag = true;
                        if (!flag)
                        {
                            db.StocktakingGoods.Add(new StocktakingGood
                            {
                                StockTakingGroup = groupNoInvertory,
                                GoodId = b.GoodId,
                                Price=b.Good.GoodPrices.FirstOrDefault().Price,
                                Count = 0,
                                CountDB = b.Count,
                                CountFact = 0,
                            });
                            b.Count = 0;
                        };
                    };
                    //Теперь учтем продажи после даты выполнения инверторизации
                    var sells = await db.Shifts.Include(s => s.CheckSells).ThenInclude(c => c.CheckGoods).ThenInclude(g => g.Good).Where(s => s.ShopId == stocktaking.ShopId & s.Start >= stocktaking.Create).ToListAsync();
                    foreach (var sell in sells)
                        foreach (var check in sell.CheckSells)
                            foreach (var good in check.CheckGoods)
                            {
                                var goodbalance = goodBalances.Where(gb => gb.GoodId == good.GoodId).FirstOrDefault();
                                if (goodbalance != null)
                                    goodbalance.Count -= good.Count;
                            }
                }
                await db.SaveChangesAsync();
            }
                /*
            {
                var goodbalances = await db.GoodBalances.Include(b => b.Good).ThenInclude(b=>b.GoodPrices.Where(p=>p.ShopId==stocktaking.ShopId)).Include(b => b.Shop).ToListAsync();
                foreach(var group in model.Groups)
                    foreach (var good in group.Goods)
                    {
                        var goodbalance = goodbalances.Where(b => b.GoodId == good.idGood & b.ShopId == stocktaking.ShopId).FirstOrDefault();
                        if (goodbalance != null)
                            goodbalance.Count = good.CountFact;
                    }
                stocktaking = await db.Stocktakings.Include(s => s.StockTakingGroups).ThenInclude(gr => gr.StocktakingGoods).Where(s => s.Id == model.id).FirstOrDefaultAsync();

                var stocktakingGoods = new List<StocktakingGood>();
                foreach (var group in stocktaking.StockTakingGroups)
                    foreach (var good in group.StocktakingGoods)
                        stocktakingGoods.Add(good);
                var groupNoFact = new StockTakingGroup { Stocktaking = stocktaking, Name = "Не учтено" };
                db.StockTakingGroups.Add(groupNoFact);
                foreach (var goodbalance in goodbalances)
                    if (stocktakingGoods.Count(s => s.GoodId == goodbalance.GoodId) == 0 & goodbalance.Count!=0)
                    {

                        db.StocktakingGoods.Add(new StocktakingGood
                        {
                            StockTakingGroup=groupNoFact,
                            Good = goodbalance.Good,
                            Count = 0,
                            CountDB = goodbalance.Count,
                            CountFact = 0,
                            Price = goodbalance.Good.GoodPrices.FirstOrDefault().Price
                        });
                        goodbalance.Count = 0;
                    };
                //Теперь учтем продажи после даты выполнения инверторизации
                var sells = await db.Shifts.Include(s => s.CheckSells).ThenInclude(c => c.CheckGoods).ThenInclude(g => g.Good).Where(s => s.ShopId == stocktaking.ShopId & s.Start >= stocktaking.Create).ToListAsync();
                foreach(var sell in sells)
                    foreach(var check in sell.CheckSells)
                        foreach(var good in check.CheckGoods)
                        {
                            var goodbalance = goodbalances.Where(gb => gb.GoodId == good.GoodId).FirstOrDefault();
                            if (goodbalance != null)
                                goodbalance.Count -= good.Count;
                        }    
            }
                */
            //Подведем итоги инверторизации и запишем итоговые значение
            stocktaking = await db.Stocktakings.Where(s => s.Id == model.id).Include(s => s.StockTakingGroups).ThenInclude(gr => gr.StocktakingGoods).FirstOrDefaultAsync();
            stocktaking.CountDb = stocktaking.StockTakingGroups.Sum(s => s.StocktakingGoods.Sum(g => g.CountDB));
            stocktaking.CountFact = stocktaking.StockTakingGroups.Sum(s => s.StocktakingGoods.Sum(g => g.CountFact));
            stocktaking.SumDb = stocktaking.StockTakingGroups.Sum(s => s.StocktakingGoods.Sum(g =>(decimal) g.CountDB * g.Price));
            stocktaking.SumFact = stocktaking.StockTakingGroups.Sum(s => s.StocktakingGoods.Sum(g => (decimal)g.CountFact * g.Price));
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(int id)
        {
            var stocktaking = await db.Stocktakings.Where(s => s.Id == id).FirstOrDefaultAsync();
            if (stocktaking.Status != DocumentStatus.Confirm)
                stocktaking.Status = DocumentStatus.Remove;
            await db.SaveChangesAsync();
            return await Index();
        }

        [HttpGet]
        public async Task<IActionResult> Print(int id)
            => View(await db.Stocktakings.Include(s => s.Shop).Include(s => s.StockTakingGroups).ThenInclude(g => g.StocktakingGoods).ThenInclude(g => g.Good).Where(s => s.Id == id).FirstOrDefaultAsync());
    }
}
