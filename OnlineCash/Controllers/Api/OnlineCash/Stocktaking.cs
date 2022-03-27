using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Services;
using OnlineCash.Models;
using OnlineCash.Filters;

namespace OnlineCash.Controllers.Api.OnlineCash
{
    [Route("api/onlinecash/[controller]")]
    [ApiController]
    [TypeFilterAttribute(typeof(ControlDocSynchFilter))]
    [TypeFilterAttribute(typeof(ControlExceptionDocSynchFilter))]
    public class Stocktaking : ControllerBase
    {
        IStockTackingService _service;
        public Stocktaking(IStockTackingService service)
        {
            _service = service;
        }

        [HttpPost("start/{shopId}")]
        public async Task<IActionResult> Start(int shopId, [FromBody] StocktakingReciveDataModel model)
        {
            await _service.StartFromOnlineCash(shopId, model);
            return Ok();
        }

        [HttpPost("stop/{uuid}")]
        public async Task<IActionResult> Stop(Guid uuid, [FromBody] List<StocktakingGroupReciveDataModel> groups)
        {
            await _service.StopFromOnlineCash(uuid, groups);
            return Ok();
        }

        [HttpPost("{shopId}")]
        public async Task<IActionResult> Save(int shopId, [FromBody] StocktakingReciveDataModel model)
        {
            try
            {
                await _service.SaveFromOnlinCash(shopId, model);
                return Ok();
            }
            catch(DbUpdateException ex)
            {
                return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
