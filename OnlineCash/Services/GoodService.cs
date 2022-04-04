using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Services
{
    public class GoodService
    {
        shopContext _db;
        GoodCountBalanceService _balanceService;
        NotificationOfEventInSystemService _notification;
        public GoodService(shopContext db, GoodCountBalanceService balanceService, NotificationOfEventInSystemService notification)
        {
            _db = db;
            _balanceService = balanceService;
            _notification = notification;
        }

        public async Task<Good> Add(Good model)
        {
            var goods = await _db.Goods.Include(g => g.BarCodes).ToListAsync();
            if (goods.Where(g => g.Uuid == model.Uuid).FirstOrDefault() != null)
                throw new Exception($"Товар {model.Name} с Uuid {model.Uuid} уже существует");
            foreach (var goodDb in goods)
                foreach (var barcodeDb in goodDb.BarCodes)
                    foreach (var barcodeModel in model.BarCodes)
                        if (barcodeDb.Code == barcodeModel.Code)
                            throw new Exception($"Штрих код {barcodeModel.Code} уже сущуствует");
            var good = new Good
            {
                Uuid = model.Uuid,
                Name = model.Name,
                Article = model.Article,
                BarCode = model.BarCode,
                Unit = model.Unit,
                SpecialType = model.SpecialType,
                VPackage = model.VPackage,
                Price = model.Price,
                GoodGroup=model.GoodGroup,
                Supplier=model.Supplier
            };
            _db.Goods.Add(good);
            foreach (var goodPrice in model.GoodPrices)
                _db.GoodPrices.Add(new GoodPrice {
                    Good=good,
                    ShopId=goodPrice.ShopId,
                    Price=goodPrice.Price,
                    BuySuccess=goodPrice.BuySuccess
                });
            foreach (var barcode in model.BarCodes)
                _db.BarCodes.Add(new BarCode { Good = good, Code = barcode.Code });
            await _db.SaveChangesAsync();

            await _balanceService.NewGood(good.Id);
            await _notification.Send(@$"Новый товар {good.Name}
Цена {good.Price}
Ед измерения {good.Unit.GetDescription()}", "Goods/Details/" + good.Id);
            return good;
        }

        public async Task Edit( Good model)
        {
            var good = await _db.Goods.Include(g => g.BarCodes).Include(g => g.GoodPrices)
                .Where(g => g.Id == model.Id || (model.Uuid!=Guid.Empty && g.Uuid == model.Uuid)).FirstOrDefaultAsync();

            if (good == null)
                throw new Exception($"Товар с id - {model.Id} или Uuid - {model.Uuid} не найден");
            decimal priceOld = good.Price;
            good.Name = model.Name;
            good.Article = model.Article;
            string barcodeForGood = model.BarCodes.FirstOrDefault()?.Code;
            good.BarCode = barcodeForGood;
            good.Unit = model.Unit;
            good.SpecialType = model.SpecialType;
            good.VPackage = model.VPackage;
            good.Price = model.Price;
            if(model.GoodGroup!=null)
                good.GoodGroup = model.GoodGroup;
            if (model.Supplier != null)
                good.Supplier = model.Supplier;
            bool flagCreateRevaluation = false;
            foreach(var price in model.GoodPrices)
            {
                var pricedb = good.GoodPrices.Where(p => p.Id == price.Id).FirstOrDefault();
                if (pricedb.Price != price.Price)
                {
                    flagCreateRevaluation = true;
                    pricedb.Price = price.Price;
                }
            };
            //Определим удаленные штриходы
            foreach (var codeDb in good.BarCodes)
            {
                var flagNotIn = true;
                foreach (var code in model.BarCodes.Where(b => b.Id != 0).ToList())
                    if (codeDb.Id == code.Id)
                        flagNotIn = false;
                if (flagNotIn)
                    _db.BarCodes.Remove(codeDb);
            }
            //Изменим текущие штрих коды
            foreach (var barcode in model.BarCodes.Where(b => b.Id != 0 & b.Id != -1).ToList())
            {
                var code = good.BarCodes.Where(b => b.Id == barcode.Id).FirstOrDefault();
                if (code != null)
                    code.Code = barcode.Code;
            }
            //Добавим новые штрих коды
            foreach (var barcode in model.BarCodes.Where(b => b.Id == 0 || b.Id == -1).ToList())
                _db.BarCodes.Add(new BarCode
                {
                    Good = good,
                    Code = barcode.Code
                });

            await _db.SaveChangesAsync();

            //Revaluation revaluation = null;
            string revaluationNotify = "";
            if (flagCreateRevaluation == true)
            {
                var revaluation = await _db.Revaluations.Where(r => DateTime.Compare(r.Create.Date, DateTime.Now) == 0).FirstOrDefaultAsync();
                if(revaluation==null)
                {
                    revaluation = new Revaluation { Create = DateTime.Now, Status = DocumentStatus.Confirm };
                    _db.Revaluations.Add(revaluation);
                };
                decimal countCurrent = (await _db.GoodCountBalanceCurrents.Where(g => g.GoodId == good.Id).FirstOrDefaultAsync())?.Count ?? 0;
                _db.RevaluationGoods.Add(new RevaluationGood
                {
                    Revaluation = revaluation,
                    Good = good,
                    PriceOld=priceOld,
                    PriceNew=good.Price,
                    Count=countCurrent
                });
                revaluation.SumOld += priceOld * countCurrent;
                revaluation.SumNew += good.Price * countCurrent;
                await _db.SaveChangesAsync();
                revaluationNotify = $"Переоценка {good.Name}. Было {priceOld * countCurrent} стало {good.Price * countCurrent}";
            }
            await _notification.Send($"Изменен товар {good.Name}. Цена с {priceOld} на {model.Price}",
                "Goods/Details/"+good.Id);
            if(flagCreateRevaluation==true)
                await _notification.Send(revaluationNotify, "Goods/Details/" + good.Id);
        }
    }
}
