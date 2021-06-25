using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using DataBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RMK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsSynchController : ControllerBase
    {
        public IConfiguration configuration;
        public ILogger<GoodsSynchController> logger;
        public onlinecashContext db;
        int ShopId;
        string serverUrl = "";
        public GoodsSynchController(IConfiguration configuration, ILogger<GoodsSynchController> logger, onlinecashContext db)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.db = db;
            ShopId = Convert.ToInt32(this.configuration.GetSection("ShopId").Value);
            serverUrl = this.configuration.GetSection("UrlServer").Value;
        }
        public async Task<IActionResult> Get()
        {
            //http://localhost:5000/api/GoodsSynch/1
            var str=await new HttpClient().GetAsync($"{serverUrl}/api/GoodsSynch/"+ShopId).Result.Content.ReadAsStringAsync();
            var goodImport = JsonSerializer.Deserialize<List<Good>>(str, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            var goods = await db.Goods.Include(g=>g.GoodPrices).ToListAsync();
            foreach(var gImport in goodImport)
            {
                bool flag = false;
                foreach(var g in goods)
                    if(gImport.Uuid==g.Uuid)
                    {
                        flag = true;
                        if (gImport.Name != g.Name)
                            g.Name = gImport.Name;
                        if (gImport.Price != g.Price)
                        {
                            g.Price = gImport.Price;
                            var price = await db.GoodPrices.Where(p => p.ShopId == ShopId & p.GoodId == g.Id).FirstOrDefaultAsync();
                            price.Price = gImport.GoodPrices.Where(p=>p.ShopId==ShopId).FirstOrDefault().Price;
                        };
                        foreach (var p in g.GoodPrices)
                            foreach (var pImport in gImport.GoodPrices)
                                if (p.GoodId == pImport.GoodId & p.ShopId == pImport.ShopId)
                                    p.Price = pImport.Price;
                        foreach (var p in g.GoodPrices)
                        {
                            var price = gImport.GoodPrices.Where(price => price.Good.Uuid == p.Good.Uuid & price.ShopId == p.ShopId).FirstOrDefault()?.Price;
                            if (price != null)
                                p.Price = (decimal)price;
                        }
                        if (gImport.BarCode != g.BarCode) 
                            g.BarCode = gImport.BarCode;
                    };
                if (!flag)
                {
                    var good = new Good
                    {
                        Name = gImport.Name,
                        Uuid = gImport.Uuid,
                        Article = gImport.Article,
                        BarCode = gImport.BarCode,
                        Unit = gImport.Unit,
                        Price = gImport.Price
                    };
                    var price_ = gImport.GoodPrices.Where(p => p.ShopId == ShopId).FirstOrDefault().Price;
                    var price = new GoodPrice { Good = good, Shop = await db.Shops.Where(s => s.Id == 1).FirstOrDefaultAsync(), Price = price_ };
                    db.Goods.Add(good);
                    db.GoodPrices.Add(price);
                }
            }
            db.SaveChanges();

            //Синхронизация смен
            var with = DateTime.Now.Date;
            var by = DateTime.Now.Date;
            var shifts = await db.Shifts
                .Include(s => s.CheckSells)
                .ThenInclude(c => c.CheckGoods)
                .ThenInclude(cg => cg.Good)
            .Where(s => s.Start > with & s.Start < by.AddDays(1)).ToListAsync();
            var shiftsModel = new List<Models.ShiftModel>();
            foreach (var shift in shifts)
            {
                var shiftModel = new Models.ShiftModel
                {
                    Start = shift.Start,
                    Stop = (DateTime)shift.Stop,
                    SumAll = shift.SumAll,
                    SumIncome = shift.SumIncome,
                    SummReturn = shift.SummReturn,
                    SumOutcome = shift.SumOutcome,
                    SumSell = shift.SumSell,
                    CheckSells = new List<Models.CheckSellModel>()
                };
                foreach (var check in shift.CheckSells)
                {
                    var checkModel = new Models.CheckSellModel
                    {
                        DateCreate = check.DateCreate,
                        IsElectron = check.IsElectron,
                        Sum = check.Sum,
                        SumDiscont = check.SumDiscont,
                        SumAll = check.SumAll,
                        Goods = new List<Models.CheckGoodModel>()
                    };
                    foreach (var checkGood in check.CheckGoods)
                        checkModel.Goods.Add(new Models.CheckGoodModel
                        {
                            Uuid = checkGood.Good.Uuid,
                            Cost = checkGood.Cost,
                            Count = checkGood.Count
                        });
                    shiftModel.CheckSells.Add(checkModel);
                }
                shiftsModel.Add(shiftModel);
            }
            try
            {
                string jsonRequest = JsonSerializer.Serialize(shiftsModel);
                string jsonResponse = await ClientServer.PostRequestAsync($"{serverUrl}/api/Shifts", jsonRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //ViewModels.ShiftViewModel.PostOnServer(db, serverUrl, DateTime.Now, DateTime.Now);

            return Ok();
        }
    }
}
