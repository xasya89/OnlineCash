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
        public IActionResult Index()
        {
            DiscountParamContainerModel model;
            if (!io.File.Exists("discounts.json"))
                using (io.StreamWriter writer = new io.StreamWriter("discounts.json"))
                {
                    model = new DiscountParamContainerModel {
                        SumBuys = new List<DiscountParamSumBuyModel>()
                        {
                            new DiscountParamSumBuyModel{IsEnable=false },
                            new DiscountParamSumBuyModel{IsEnable=true },
                            new DiscountParamSumBuyModel{IsEnable=false },
                            new DiscountParamSumBuyModel{IsEnable=true },
                        }
                    };
                    writer.Write(JsonSerializer.Serialize(model));
                }
            else
                using (io.StreamReader reader = new io.StreamReader("discounts.json"))
                    model = JsonSerializer.Deserialize<DiscountParamContainerModel>(reader.ReadToEnd());
            return View(model);
        }
    }
}
