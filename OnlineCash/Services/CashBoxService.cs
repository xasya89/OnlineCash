
using OnlineCash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using OnlineCash.Models.CashBox;

namespace OnlineCash.Services
{
    public class CashBoxService : ICashBoxService
    {
        shopContext db;
        CashMoneyService _moneyService;
        MoneyBalanceService _moneyBalanceService;
        GoodCountBalanceService _countBalanceService;
        public CashBoxService(shopContext db,
            MoneyBalanceService moneyBalanceService, 
            CashMoneyService moneyService,
            GoodCountBalanceService countBalanceService)
        {
            this.db = db;
            _moneyService = moneyService;
            _moneyBalanceService = moneyBalanceService;
            _countBalanceService = countBalanceService;
        }
        //TODO: Возможно данный метод не нужен
        public async Task<bool> Buy(Guid uuid, List<CashBoxBuyReturnModel> buylist)
        {
            var shift = await db.Shifts.Where(s => s.Uuid == uuid & s.Stop == null).FirstOrDefaultAsync();
            if (shift == null)
                return false;
            var goods = db.Goods.ToList();
            foreach (var buy in buylist)
            {
                var good = goods.Where(g => g.Uuid == buy.Uuid).FirstOrDefault();
                if (good == null)
                    return false;
                var sale = await db.ShiftSales.Where(s => s.ShiftId == shift.Id & s.GoodId == good.Id & s.Price == buy.Price).FirstOrDefaultAsync();
                if (sale != null)
                    sale.Count += buy.Count;
                else
                    db.ShiftSales.Add(new ShiftSale
                    {
                        Shift = shift,
                        Good = good,
                        Price = buy.Price,
                        Count = buy.Count
                    });
            }
            shift.SumSell += buylist.Sum(b => (decimal)b.Count * b.Price);
            shift.SumElectron+=buylist.Where(b=>b.IsElectron).Sum(b => (decimal)b.Count * b.Price);
            shift.SumNoElectron+=buylist.Where(b=>b.IsElectron==false).Sum(b => (decimal)b.Count * b.Price);
            shift.SumAll+= buylist.Sum(b => (decimal)b.Count * b.Price);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task Sell(Guid uuid, CashBoxCheckSellModel check)
        {
            var shift = await db.Shifts.Where(s => s.Uuid == uuid & s.Stop == null).FirstOrDefaultAsync();
            if (shift == null)
                throw new Exception($"Смена с uuid {uuid} не найдена");
            var goods = await db.Goods.ToListAsync();
            foreach (var good in check.Goods)
                if (goods.Where(g => g.Uuid == good.Uuid).FirstOrDefault() == null)
                    throw new Exception($"Товар с uuid {good.Uuid} не найден");
            foreach(var checkGood in check.Goods)
            {
                var good = goods.Where(g => g.Uuid == checkGood.Uuid).FirstOrDefault();
                var sale = await db.ShiftSales.Where(s => s.ShiftId == shift.Id & s.GoodId == good.Id & s.Price == checkGood.Price).FirstOrDefaultAsync();
                if (check.IsReturn == false)
                {
                    if (sale != null)
                        sale.Count += (double)checkGood.Count;
                    else
                        db.ShiftSales.Add(new ShiftSale
                        {
                            Shift = shift,
                            Good = good,
                            Price = checkGood.Price,
                            Count = (double)checkGood.Count
                        });
                    await _countBalanceService.AddSell(shift.Id, shift.Start, good.Id, checkGood.Count);
                    //await goodBalanceService.MinusAsync(shift.ShopId, good.Id, (double)checkGood.Count);
                }
                if (check.IsReturn == true)
                {
                    if (sale != null)
                        sale.CountReturn += checkGood.Count;
                    else
                        db.ShiftSales.Add(new ShiftSale
                        {
                            Shift = shift,
                            Good = good,
                            PriceReturn = checkGood.Price,
                            CountReturn = checkGood.Count
                        });
                    await _countBalanceService.AddReturn(shift.Id, shift.Start, good.Id, checkGood.Count);
                    //await goodBalanceService.PlusAsync(shift.ShopId, good.Id, (double)checkGood.Count);
                }
            }
            if (check.IsReturn == false)
            {
                shift.SumSell += check.SumCash + check.SumElectron - check.SumDiscount;
                shift.SumElectron += check.SumElectron;
                shift.SumNoElectron += check.SumCash;
                shift.SumAll += check.SumCash + check.SumElectron - check.SumDiscount;
            }
            if (check.IsReturn == true)
            {
                shift.SumReturnElectron += check.SumElectron;
                shift.SumReturnCash += check.SumCash;
                shift.SumAll -= check.SumCash + check.SumElectron;
            }
            //Добавим чек
            Buyer buyer = null;
            if (check.Buyer != null)
            {
                buyer = await db.Buyers.Where(b => b.Uuid == check.Buyer.Uuid).FirstOrDefaultAsync();
                if (buyer == null)
                {
                    buyer = new Buyer
                    {
                        Uuid = check.Buyer.Uuid,
                        Phone = check.Buyer.Phone,
                        SumBuy = check.IsReturn ? 0 : check.SumCash + check.SumElectron
                    };
                    db.Buyers.Add(buyer);
                }
            }
            var checksell = new CheckSell { 
                DateCreate=check.Create,
                TypeSell = check.IsReturn ? TypeSell.Return : TypeSell.Sell,
                Shift=shift,
                Buyer =buyer,
                SumCash=check.SumCash,
                SumElectron=check.SumElectron,
                Sum=check.SumCash + check.SumElectron,
                SumDiscont=check.SumDiscount,
                SumAll= check.SumCash + check.SumElectron - check.SumDiscount
            };
            db.CheckSells.Add(checksell);
            foreach(var checkGood in check.Goods)
            {
                var good = goods.Where(g => g.Uuid == checkGood.Uuid).FirstOrDefault();
                db.CheckGoods.Add(new CheckGood
                {
                    CheckSell = checksell,
                    Good = good,
                    Count = checkGood.Count,
                    Price = checkGood.Price
                });
            }

            await db.SaveChangesAsync();
            if (check.SumCash > 0 && check.IsReturn==false)
                await _moneyBalanceService.AddSale(shift.ShopId, check.SumCash);
            if (check.SumCash > 0 && check.IsReturn == true)
                await _moneyBalanceService.AddReturn(shift.ShopId, check.SumCash);
        }

        public async Task<Shift> CloseShift(Guid uuid, DateTime stop)
        {
            var shift = await db.Shifts.Where(s => s.Uuid==uuid & s.Stop == null).FirstOrDefaultAsync();
            if (shift == null)
                throw new Exception($"Смена uuid - {uuid} не найдена или закрыта");
            shift.Stop = stop;
            await db.SaveChangesAsync();
            await _moneyService.Add(shift.ShopId, new CashMoney {
                Create = stop,
                TypeOperation = CashMoneyTypeOperations.Sale,
                Sum = shift.SumNoElectron,
                Note = $"Смена № {shift.Id} от {shift.Start.ToString("dd.MM.yy")}"
            });
            if(shift.SumReturnCash>0)
                await _moneyService.Add(shift.ShopId, new CashMoney
                {
                    Create = stop,
                    TypeOperation = CashMoneyTypeOperations.Return,
                    Sum = shift.SumReturnCash,
                    Note = $"Смена возврат № {shift.Id} от {shift.Start.ToString("dd.MM.yy")}"
                });
            return shift;
        }

        public async Task<bool> OpenShift(int idShop, Guid uuid, DateTime start)
        {
            try
            {
                var shift = await db.Shifts.Where(s => s.ShopId == idShop & s.Stop==null).FirstOrDefaultAsync();
                if (shift != null)
                    throw new Exception("Нет закрытых смен");
                var cashier = await db.Cashiers.FirstOrDefaultAsync();
                var shop = await db.Shops.Where(s => s.Id == idShop).FirstOrDefaultAsync();
                if (shop == null)
                    throw new Exception("Магазин не найден");
                db.Shifts.Add(new Shift
                {
                    Uuid=uuid,
                    ShopId = idShop,
                    CashierId = cashier.Id,
                    Start = start
                });
                await db.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public Task<bool> Return(int idShop, List<CashBoxBuyReturnModel> buylist)
        {
            throw new NotImplementedException();
        }
    }
}
