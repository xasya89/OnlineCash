using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;
using OnlineCash.Services;

namespace OnlineCash.Controllers.Api
{
    //TODO: Возможно старый контроллер, который получал из кассы отчет за смену в конце смены. Нужно разобраться и удалить
    [Route("api/[controller]")]
    [ApiController]
    public class Shifts : ControllerBase
    {
        public IConfiguration configuration;
        public ILogger<Shifts> logger;
        public shopContext db;
        IGoodBalanceService goodBalanceService;
        public Shifts(IConfiguration configuration, ILogger<Shifts> logger, shopContext db, IGoodBalanceService goodBalanceService)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.db = db;
            this.goodBalanceService = goodBalanceService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]List<Models.ShiftModel> shifts)
        {
            try
            {
                var shops = await db.Shops.ToListAsync();
                var goods = await db.Goods.ToListAsync();
                foreach (var shift in shifts)
                {
                    var shop = shops.Where(s => s.Id == shift.ShopId).FirstOrDefault();
                    if (shop == null)
                        return BadRequest();
                    foreach (var check in shift.CheckSells)
                        foreach (var checkGood in check.Goods)
                            if (goods.Where(g => g.Uuid == Guid.Parse( checkGood.Uuid)).FirstOrDefault() == null)
                                return BadRequest();
                };

                var cashier = await db.Cashiers.FirstOrDefaultAsync();
                foreach(var shift in shifts)
                {
                    var shop = shops.Where(s => s.Id == shift.ShopId).FirstOrDefault();
                    var shiftdb = new Shift
                    {
                        Cashier = cashier,
                        Shop = shop,
                        Start = shift.Start,
                        Stop = shift.Stop,
                        SumIncome = shift.SumIncome,
                        //SummReturn = shift.SummReturn,
                        SumOutcome = shift.SumOutcome,
                        SumSell = shift.SumSell,
                        SumAll = shift.SumAll
                    };
                    db.Shifts.Add(shiftdb);
                    foreach(var check in shift.CheckSells)
                    {
                        var checkdb = new CheckSell
                        {
                            Shift = shiftdb,
                            DateCreate = check.DateCreate,
                            IsElectron = check.IsElectron,
                            Sum = check.Sum,
                            SumDiscont = check.SumDiscont,
                            SumAll = check.SumAll
                        };
                        db.CheckSells.Add(checkdb);
                        foreach (var checkGood in check.Goods)
                        {
                            var good = goods.Where(g => g.Uuid == Guid.Parse( checkGood.Uuid)).FirstOrDefault();
                            var checgooddb = new CheckGood
                            {
                                CheckSell = checkdb,
                                Good = good,
                                Price = checkGood.Cost,
                                Count =(decimal) checkGood.Count
                            };
                            db.CheckGoods.Add(checgooddb);
                            var goodBalance = await db.GoodBalances.Where(b => b.ShopId == shop.Id & b.GoodId == good.Id).FirstOrDefaultAsync();
                            if (goodBalance == null)
                            {
                                var goodBalanceNew = new GoodBalance
                                {
                                    Shop = shop,
                                    Good = good,
                                    Count = -1 * checkGood.Count
                                };
                                db.GoodBalances.Add(goodBalanceNew);
                            }
                            else
                                goodBalance.Count -= checkGood.Count;
                        }
                    };
                }
                await db.SaveChangesAsync();
                /*
                await db.Shifts.AddRangeAsync(shifts);
                await db.SaveChangesAsync();
                */
                return Ok();
            }
            catch(Exception ex) 
            {
                logger.LogError(ex.Message);
                return BadRequest(); 
            };
        }

        [HttpPost("One")]
        public async Task<IActionResult> PostOne([FromBody] ShiftModel shift)
        {
            try
            {
                var shops = await db.Shops.ToListAsync();
                var goods = await db.Goods.ToListAsync();
                var shop = shops.Where(s => s.Id == shift.ShopId).FirstOrDefault();
                if (shop == null)
                    return BadRequest("Shop is null");
                foreach (var check in shift.CheckSells)
                    foreach (var checkGood in check.Goods)
                        if (goods.Where(g => g.Uuid == Guid.Parse(checkGood.Uuid)).FirstOrDefault() == null)
                        {
                            System.Diagnostics.Debug.WriteLine(checkGood.Uuid);
                            return BadRequest();
                        }

                var cashier = await db.Cashiers.FirstOrDefaultAsync();
                var goodsdb = await db.Goods.ToListAsync();

                List<ShiftSale> sales = new List<ShiftSale>();
                foreach(var shiftcheck in shift.CheckSells)
                    foreach(var checksale in shiftcheck.Goods)
                    {
                        var gooddb = goodsdb.Where(g => g.Uuid.ToString() == checksale.Uuid).FirstOrDefault();
                        if (gooddb == null)
                            return BadRequest($"Товар с uuid {checksale.Uuid} не найден");
                        else
                        {
                            var sale = sales.Where(s => s.GoodId == gooddb.Id & s.Price==checksale.Cost).FirstOrDefault();
                            if (sale == null)
                                sales.Add(new ShiftSale { GoodId = gooddb.Id, Price = checksale.Cost, Count = checksale.Count });
                            else
                                sale.Count += checksale.Count;
                        }
                    }
                var shiftdb = new Shift
                {
                    Cashier = cashier,
                    Shop = shop,
                    Start = shift.Start,
                    Stop = shift.Stop,
                    SumIncome = shift.SumIncome,
                    //SummReturn = shift.SummReturn,
                    SumOutcome = shift.SumOutcome,
                    SumNoElectron = shift.SumNoElectron,
                    SumElectron = shift.SumElectron,
                    SumSell = shift.SumSell,
                    SumAll = shift.SumAll
                };
                db.Shifts.Add(shiftdb);
                foreach(var sale in sales)
                {
                    sale.Shift = shiftdb;
                    db.ShiftSales.Add(sale);
                }
                await db.SaveChangesAsync();

                await goodBalanceService.CalcAsync(shop.Id,shift.Stop);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest();
            };
        }
    }
}
