using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;
using OnlineCash.Services;

namespace OnlineCash.Controllers.Api.OnlineCash
{
    [Route("api/onlinecash/[controller]")]
    [ApiController]
    public class NewGoodFromCashSynch : ControllerBase
    {
        shopContext _db;
        NotificationOfEventInSystemService _notification;
        public NewGoodFromCashSynch(shopContext db, NotificationOfEventInSystemService notification)
        {
            _db = db;
            _notification = notification;
        }

        [HttpPost("{shopId}")]
        public async Task<IActionResult> Post(int shopId, [FromBody] GoodSynchModel model)
        {
            var goodOldDb = await _db.Goods.Where(g => g.Uuid == model.Uuid).FirstOrDefaultAsync();
            /*
            if (goodOldDb != null)
                return BadRequest();
            */
            if(goodOldDb!=null)
            {
                goodOldDb.Price = model.Price;
                var price = await _db.GoodPrices.Where(p => p.GoodId == goodOldDb.Id & p.ShopId == shopId).FirstOrDefaultAsync();
                decimal oldPrice = price.Price;
                price.Price = model.Price;
                var newGoodFromCash = new NewGoodFromCash { Good = goodOldDb, IsNewGood = false };
                _db.NewGoodFromCashes.Add(newGoodFromCash);
                await _db.SaveChangesAsync();
                await _notification.Send($"Изменен товар {goodOldDb.Name}. Изменилась цена с {oldPrice} на {model.Price}", "NewGoodsFromCash/open/"+newGoodFromCash.Id);
                return Ok();
            }
            List<string> barcodes = new List<string>();
            foreach (var code in model.Barcodes)
                barcodes.Add(code);
            var barcodeDb = await _db.BarCodes.Where(b => barcodes.Contains(b.Code)).FirstOrDefaultAsync();
            if (barcodeDb != null)
                return BadRequest();
            var groupDb = await _db.GoodGroups.FirstOrDefaultAsync();
            var good = new Good
            {
                Uuid = model.Uuid,
                Name = model.Name,
                Unit = model.Unit,
                Price = model.Price,
                GoodGroup = groupDb,
                IsDeleted = false
            };
            _db.Goods.Add(good);
            var shops = _db.Shops.ToList();
            foreach (var shop in shops)
            {
                _db.GoodPrices.Add(new GoodPrice { Good = good, Shop = shop, Price = model.Price, BuySuccess = true });
                var goodBalance = new GoodBalance
                {
                    Shop = shop,
                    Good = good,
                    Count = 0
                };
                _db.GoodBalances.Add(goodBalance);
            }
            foreach (var barcode in model.Barcodes)
                _db.BarCodes.Add(new BarCode { Good = good, Code = barcode });
            var newGoodFromCashNew = new NewGoodFromCash { Good = good, IsNewGood = true };
            _db.NewGoodFromCashes.Add(newGoodFromCashNew);
            await _db.SaveChangesAsync();
            await _notification.Send(@$"Новый товар {good.Name}
Цена {good.Price}
Ед измерения {good.Unit.GetDescription()}", "NewGoodsFromCash/open/" + newGoodFromCashNew.Id);
            return Ok();
        }
    }
}
