using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RMK.DataBaseModels;
using System.Text.Json;

namespace RMK.Controllers
{
    public class StocktakingController : Controller
    {
        onlinecashContext db;
        ILogger<StocktakingController> logger;
        IConfiguration configuration;
        int ShopId;
        string serverUrl = "";
        public StocktakingController(onlinecashContext db, ILogger<StocktakingController> logger, IConfiguration configuration)
        {
            this.db = db;
            this.logger = logger;
            this.configuration = configuration;
            ShopId = Convert.ToInt32(configuration.GetSection("ShopId").Value);
            serverUrl = this.configuration.GetSection("UrlServer").Value;
        }

        public async Task<ActionResult> Index()
        {
            var stocktaking = await db.Stocktakings.FirstOrDefaultAsync(s => s.isComplite == false);
            if (stocktaking == null)
            {
                int? num = await db.Stocktakings.MaxAsync(s => (int?)s.Num);
                stocktaking = new Stocktaking
                {
                    Num = num!=null ? (int)num+1 : 1,
                    Create = DateTime.Now,
                    ShopId=ShopId
                };
                db.Stocktakings.Add(stocktaking);
                await db.SaveChangesAsync();
            };
            return View(stocktaking);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Stocktaking model)
        {
            await SaveInDB(model);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> SaveAndSend([FromBody] Stocktaking model)
        {
            await SaveInDB(model);
            var stotaking = await db.Stocktakings.Where(s => s.Id == model.Id).FirstOrDefaultAsync();
            //Отправка на сервер
            try
            {
                string jsonRequest = JsonSerializer.Serialize(stotaking);
                string jsonResponse = await ClientServer.PostRequestAsync($"{serverUrl}/api/Stocktakings", jsonRequest);
                return Ok(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Ok();
        }

        async Task<Stocktaking> SaveInDB(Stocktaking model)
        {
            var stocktaking = await db.Stocktakings.Where(s => s.Id == model.Id).FirstOrDefaultAsync();
            var stocktakingGoods = await db.StocktakingGoods.Where(s => s.StocktakingId == model.Id).ToListAsync();
            //Изменим количество в ранее добавленных товарах
            foreach (var stgood in stocktakingGoods)
                foreach (var st in model.StocktakingGoods.Where(s => s.Id != -1).ToList())
                    if (stgood.Id == st.Id & stgood.Count != st.Count)
                        stgood.Count = st.Count;
            //Добавим новые товары
            var goods = await db.Goods.ToListAsync();
            foreach (var stgood in model.StocktakingGoods.Where(s => s.Id == -1).ToList())
            {
                Guid uuid = goods.Where(g => g.Id == stgood.idGood).FirstOrDefault().Uuid;

                var stGood = new StocktakingGood
                {
                    idGood = stgood.idGood,
                    GoodUuid = uuid,
                    Count = stgood.Count,
                    Stocktaking = stocktaking
                };
                db.StocktakingGoods.Add(stGood);
            }
            await db.SaveChangesAsync();
            return await db.Stocktakings.Where(s => s.Id == model.Id).FirstOrDefaultAsync();
        }
    }
}
