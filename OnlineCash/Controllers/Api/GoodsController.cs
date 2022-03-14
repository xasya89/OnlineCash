using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        public shopContext db;
        public GoodsController( shopContext db)
        {
            this.db = db;
        }
        [HttpGet]
        //TODO: Создано временное решение для выыборки только не удаленных товаров, т.к. ошибка MySQL server version for the right syntax to use near 'NOT 
        public async Task<IActionResult> Get(string Name)
            => Ok((
                await db.Goods
                .Include(g=>g.BarCodes)
                .Include(g=>g.GoodPrices)
                .Include(g=>g.GoodGroup)
                .Include(g=>g.Supplier)
                .Where(g => EF.Functions.Like(g.Name, $"%{Name}%")).ToListAsync()
                ).Where(g=>g.IsDeleted==false).ToList());

        [HttpGet("{id}")]
        public async Task<IActionResult> Default(int id)
            => Ok(await db.Goods
                .Include(g => g.BarCodes)
                .Include(g => g.GoodPrices)
                .Include(g => g.GoodGroup)
                .Include(g=>g.Supplier)
                .Where(g => g.Id == id).FirstOrDefaultAsync());

        [HttpGet("{id}/{idShop}")]
        public async Task<IActionResult> DefaultShop(int id, int idShop)
        {
            var good = await db.Goods
                .Include(g=>g.BarCodes)
                .Include(g=>g.GoodPrices)
                .Include(g => g.GoodGroup)
                .Include(g => g.Supplier)
                .Where(g => g.Id == id).FirstOrDefaultAsync();
            if (good != null)
                good.Price = good.GoodPrices.Where(p => p.ShopId == idShop).FirstOrDefault().Price;
            return Ok(good);
        }
        [HttpPost("list/{idShop}")]
        //TODO: Проверить необходимость и адекватность данной функции
        public async Task<IActionResult> DefaultList([FromBody] List<int> idGoods, int idShop)
        {
            var goods = (await db.Goods
                .Include(g=>g.BarCodes)
                .Include(g=>g.GoodGroup)
                .Include(g=>g.GoodPrices)
                .Include(g=>g.Supplier)
                .Where(g=>idGoods.Contains(g.Id) & g.IsDeleted == false).ToListAsync()).Where(g=>g.IsDeleted==false).ToList();
            foreach (var good in goods)
            {
                var goodprice = good.GoodPrices.Where(p => p.ShopId == idShop).FirstOrDefault();
                if (goodprice != null)
                    good.Price = goodprice.Price;
            }
            return Ok(goods);
        }
        [HttpGet("find/{find}")]
        public async Task<IActionResult> Find(string find) =>
            Ok((
                await db.Goods
                .Include(g=>g.BarCodes)
                .Where(g => EF.Functions.Like(g.Name, $"%{find}%")).ToListAsync()
                ).Where(g=>g.IsDeleted==false).ToList());
    }
}
