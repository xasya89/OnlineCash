using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using OnlineCash.Models.EvotorModels;

namespace OnlineCash.Services
{
    public class EvotorService : IEvotorService
    {
        IConfiguration configuration;
        shopContext db;
        public EvotorService(IConfiguration configuration, shopContext db)
        {
            this.configuration = configuration;
            this.db = db;
        }
        public async Task<bool> ExchangeGoods()
        {
            var goodsEvotor= await "https://api.evotor.ru/api/v1/inventories/stores/20200922-FB0C-4040-80A6-2959D9DF95A3/products"
                .WithHeader( "X-Authorization", "dcad7a89-1fc6-4a66-b273-354f730075bb" )
                .GetJsonAsync<List<EvotorGoodModel>>();
            var goods = await db.Goods.Include(g => g.GoodPrices).Include(g => g.BarCodes).ToListAsync();
            foreach(var goodEvotor in goodsEvotor)
                if(goodEvotor.MeasureName!=null & goodEvotor.Price!=null)
            {
                DataBase.Units unit = DataBase.Units.PCE;
                switch (goodEvotor.MeasureName.ToLower())
                {
                    case "шт":
                        unit = DataBase.Units.PCE;
                        break;
                    case "кг":
                        unit = DataBase.Units.KG;
                        break;
                    case "л":
                        unit = DataBase.Units.Litr;
                        break;
                };
                string barcode = goodEvotor.BarCodes.FirstOrDefault();
                decimal price = 0;
                if (goodEvotor.Price != null)
                    price = (decimal)goodEvotor.Price;
                var good = goods.Where(g => g.Uuid == goodEvotor.Uuid).FirstOrDefault();
                if (good==null)
                {
                    good = new Good { GoodGroupId=1, Uuid = goodEvotor.Uuid, Name = goodEvotor.Name, Unit = unit, BarCode = barcode, Price = price };
                    db.Goods.Add(good);
                };
                foreach (var bcode in good.BarCodes)
                    if (goodEvotor.BarCodes.Where(b => b == bcode.Code) == null)
                        db.BarCodes.Remove(bcode);
                foreach (var bcode in goodEvotor.BarCodes)
                    if (good.BarCodes.Where(b => b.Code == bcode) == null)
                        db.BarCodes.Add(new BarCode { Good = good, Code = bcode });
                var goodPrice = await db.GoodPrices.Where(p => p.GoodId == good.Id & p.ShopId == 1).FirstOrDefaultAsync();
                /*
                if (goodPrice == null)
                {
                    db.GoodPrices.Add(new GoodPrice { GoodId = good.Id, ShopId = 1, Price = price, BuySuccess = true });
                }
                else
                    goodPrice.Price = price;
                */
                //db.BarCodes.RemoveRange(good.BarCodes);
                await db.SaveChangesAsync();
            }
            return true;
        }
    }
}
