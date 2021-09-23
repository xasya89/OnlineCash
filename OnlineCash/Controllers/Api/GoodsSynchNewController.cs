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

namespace OnlineCash.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsSynchNewController : ControllerBase
    {
        public IConfiguration configuration;
        public ILogger<GoodsSynchNewController> logger;
        public shopContext db;
        public GoodsSynchNewController(IConfiguration configuration, ILogger<GoodsSynchNewController> logger, shopContext db)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.db = db;

            var goods = db.Goods.Where(g => g.Uuid.ToString() == "00000000-0000-0000-0000-000000000000").ToList();
            foreach (var g in goods)
                g.Uuid = Guid.NewGuid();
            db.SaveChanges();
        }

        [HttpGet("{idShop}")]
        public async Task<List<GoodSynchModel>> Get(int idShop)
        {
            List<Good> goods = await db.Goods
                .Include(g => g.BarCodes)
                .Include(g => g.GoodPrices.Where(gp => gp.ShopId == idShop))
                .ToListAsync();
            List<GoodSynchModel> goodsSynch = new List<GoodSynchModel>();
            foreach (var good in goods)
                if (good.GoodPrices.Count > 0)
                {
                    var price = good.GoodPrices.FirstOrDefault();
                    bool isDelete = good.IsDeleted;
                    if (!price.BuySuccess)
                        isDelete = true;
                    var goodSynch = new GoodSynchModel
                    {
                        Uuid = good.Uuid,
                        Name = good.Name,
                        Article = good.Article,
                        SpecialType = good.SpecialType,
                        Unit = good.Unit,
                        VPackage = good.VPackage,
                        Price = price.Price,
                        IsDeleted = isDelete
                    };
                    foreach (var barcode in good.BarCodes)
                        goodSynch.Barcodes.Add(barcode.Code);
                    goodsSynch.Add(goodSynch);
                };
            return goodsSynch;
        }

        [HttpPost]
        public async Task<Good> Post([FromBody] Good good)
        {
            good.Uuid = Guid.NewGuid();
            good.Id = 0;
            var goodGroup = db.GoodGroups.FirstOrDefault();
            good.GoodGroupId = db.GoodGroups.FirstOrDefault().Id;
            db.Goods.Add(good);
            decimal price = good.Price;
            var shops = await db.Shops.ToListAsync();
            foreach(var shop in shops)
            {
                GoodPrice goodPrice = new GoodPrice
                {
                    Good = good,
                    Shop = shop,
                    Price = price
                };
                db.GoodPrices.Add(goodPrice);
            }
            db.GoodAddeds.Add(new GoodAdded { Good = good });
            await db.SaveChangesAsync();
            return good;
        }
    }
}
