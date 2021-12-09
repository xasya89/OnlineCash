using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using io=System.IO;
using System.Text.Json;
using OnlineCash.Models.Discounts;

namespace OnlineCash.Controllers
{
    public class DiscountSettingController : Controller
    {
        const string settingFileName = "discounts.json";

        public IActionResult Index()
        {
            DiscountParamContainerModel model;
            if (!io.File.Exists("discounts.json"))
                using (io.StreamWriter writer = new io.StreamWriter(settingFileName))
                {
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

                    writer.Write(JsonSerializer.Serialize(model));
                }
            else
                using (io.StreamReader reader = new io.StreamReader(settingFileName))
                    model = JsonSerializer.Deserialize<DiscountParamContainerModel>(reader.ReadToEnd());
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] DiscountParamContainerModel model)
        {
            io.File.Delete(settingFileName);
            using (io.StreamWriter writer = new io.StreamWriter(settingFileName))
                writer.Write(JsonSerializer.Serialize(model));
            return Ok(model);
        }
    }
}
