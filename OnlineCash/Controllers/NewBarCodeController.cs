using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineCash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewBarCodeController : ControllerBase
    {
        IConfiguration configuration;
        ILogger<NewBarCodeController> logger;
        shopContext db;
        public NewBarCodeController(IConfiguration configuration, ILogger<NewBarCodeController> logger, shopContext db)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.db = db;
        }

        public async Task<string> Get()
        {
            string barCode = "";
            bool flag = true;
            while (flag)
            {
                for (int i = 0; i <= 5; i++)
                    barCode += new Random().Next(1, 9);
                var barCodeInDb = await db.Goods.FirstOrDefaultAsync(g => g.BarCode == barCode);
                if (barCodeInDb == null)
                    flag = false;
            }
            return barCode;
        }
    }
}
