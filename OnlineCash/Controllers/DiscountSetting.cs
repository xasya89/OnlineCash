using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using io=System.IO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Models.Discounts;
using DatabaseBuyer;

namespace OnlineCash.Controllers
{
    public class DiscountSettingController : Controller
    {
        private readonly shopbuyerContext _dbBuyer;
        public DiscountSettingController(shopbuyerContext dbBuyer) => _dbBuyer = dbBuyer;

        public async Task<IActionResult> Index()
        {
            var settingStr = (await _dbBuyer.DiscountSettings.FirstOrDefaultAsync())?.Settings;
            DiscountParamContainerModel model=null;
            if (settingStr != null)
                try
                {
                    model = JsonSerializer.Deserialize<DiscountParamContainerModel>(settingStr);
                }
                catch (Exception) { };
            if(model==null)
                model = new DiscountParamContainerModel
                {
                    Weeks = new List<DiscountParamWeeksModel>()
                        {
                            new DiscountParamWeeksModel{DayNum=1,DayName="Пн."},
                            new DiscountParamWeeksModel{DayNum=2,DayName="Вт."},
                            new DiscountParamWeeksModel{DayNum=3,DayName="Ср."},
                            new DiscountParamWeeksModel{DayNum=4,DayName="Чт."},
                            new DiscountParamWeeksModel{DayNum=5,DayName="Пн."},
                            new DiscountParamWeeksModel{DayNum=6,DayName="Сб."},
                            new DiscountParamWeeksModel{DayNum=7,DayName="Вс."}
                        }
                };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] DiscountParamContainerModel model)
        {
            var dbmodel = await _dbBuyer.DiscountSettings.FirstOrDefaultAsync();
            if(dbmodel==null)
            {
                dbmodel = new DiscountSetting();
                _dbBuyer.DiscountSettings.Add(dbmodel);
            }
            dbmodel.Settings = JsonSerializer.Serialize(model);
            await _dbBuyer.SaveChangesAsync();
            return Ok(model);
        }
    }
}
