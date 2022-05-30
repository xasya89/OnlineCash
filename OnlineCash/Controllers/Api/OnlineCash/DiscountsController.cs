using DatabaseBuyer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using OnlineCash.Models;

namespace OnlineCash.Controllers.Api.OnlineCash
{
    [Route("api/onlinecash/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        shopbuyerContext _dbBuyer;
        public DiscountsController(shopbuyerContext dbBuyer) => _dbBuyer = dbBuyer;

        public async Task<IActionResult> Get() =>
            Ok(JsonSerializer.Deserialize<DiscountParamContainerModel>(
                (await _dbBuyer.DiscountSettings.FirstOrDefaultAsync())?.Settings
                ));
    }
}
