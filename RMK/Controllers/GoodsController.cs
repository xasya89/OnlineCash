using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RMK.ViewModels;
using Microsoft.Extensions.Configuration;

namespace RMK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        ILogger<GoodsController> logger;
        onlinecashContext db;
        IConfiguration configuration;
        int ShopId;
        public GoodsController(ILogger<GoodsController> logger, onlinecashContext db, IConfiguration configuration)
        {
            this.logger = logger;
            this.db = db;
            this.configuration = configuration;
            this.ShopId = Convert.ToInt32(configuration.GetSection("ShopId").Value);
        }
        [HttpGet]
        public async Task<IActionResult> GetGoods()
        {
            var prices = await db.GoodPrices.Include(g => g.Good).Include(s => s.Shop).Where(p => p.ShopId == ShopId).ToListAsync();
            var goodsInShop =new List<GoodPriceInShop>();
            foreach (var p in prices)
                goodsInShop.Add(new GoodPriceInShop
                {
                    Id = p.Good.Id,
                    Name = p.Good.Name,
                    Article = p.Good.Article,
                    BarCode = p.Good.BarCode,
                    Price = p.Price
                });
            return Ok(goodsInShop);
        }
        [HttpGet("{GoodName}")]
        public async Task<IActionResult> GetGoodsByName(string GoodName)
        {
            var prices = await db.GoodPrices.Include(g => g.Good).Include(s => s.Shop).Where(p => p.ShopId == ShopId & EF.Functions.Like(p.Good.Name,$"%{GoodName}%")).ToListAsync();
            var goodsInShop = new List<GoodPriceInShop>();
            foreach (var p in prices)
                goodsInShop.Add(new GoodPriceInShop
                {
                    Id = p.Good.Id,
                    Name = p.Good.Name,
                    Article = p.Good.Article,
                    BarCode = p.Good.BarCode,
                    Price = p.Price
                });
            return Ok(goodsInShop);
        }
        [HttpGet]
        [Route("one/{GoodName}")]
        public async Task<IActionResult> GetGoodOne(string GoodName) => 
            Ok(await db.Goods.Where(g=>g.Name==GoodName).FirstOrDefaultAsync());
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGoodById(int id)
        {
            var good = await db.GoodPrices.Include(g => g.Good).Include(s => s.Shop).Where(p => p.ShopId == ShopId & p.GoodId == id).FirstOrDefaultAsync();
            if (good != null)
                return Ok(new GoodPriceInShop
                {
                    Id = good.Id,
                    Name = good.Good.Name,
                    Article = good.Good.Article,
                    BarCode = good.Good.BarCode,
                    Price = good.Price
                });
            return BadRequest();
        }
    }
}
