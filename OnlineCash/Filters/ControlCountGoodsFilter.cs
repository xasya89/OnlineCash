using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace OnlineCash.Filters
{
    public class ControlCountGoodsFilter : IActionFilter
    {
        ILogger<ControlCountGoodsFilter> _logger;
        shopContext _db;
        int goodCount = 0;
        public ControlCountGoodsFilter(ILogger<ControlCountGoodsFilter> logger, shopContext db)
        {
            _logger = logger;
            _db = db;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            /*
            int goodCountNow = _db.Goods.Count();
            if (goodCount > goodCountNow)
                _logger.LogError($"Good change {JsonSerializer.Serialize(context.RouteData.Values)} - count old: {goodCount} count new: {goodCountNow}");
            */
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //goodCount = _db.Goods.Count();
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            /*
            if (goodCount > _db.Goods.Count())
                _logger.LogError($"Good change {JsonSerializer.Serialize(context.RouteData.Values)}");
            */
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            //goodCount = _db.Goods.Count();
        }
    }
}
