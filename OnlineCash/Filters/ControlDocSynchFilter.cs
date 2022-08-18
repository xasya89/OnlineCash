using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using OnlineCash.Services;

namespace OnlineCash.Filters
{
    public class ControlDocSynchFilter : Attribute, IAsyncActionFilter
    {
        shopContext _db;
        DocSynchScopeService _docSynchService;
        public ControlDocSynchFilter(shopContext db, DocSynchScopeService docSynchScopeService)
        {
            _db = db;
            _docSynchService = docSynchScopeService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string uuidStr = context.HttpContext.Request.Headers.Where(h => h.Key.ToLower() == "doc-uuid").Select(h => h.Value).FirstOrDefault();
            if (uuidStr != null)
            {
                Guid uuid = Guid.Parse(uuidStr);

                _db.DocSynches.Add(_docSynchService.NewDocSynch(uuid));
                await _db.SaveChangesAsync();
                /*
                if (await _db.DocSynches.Where(d => d.Uuid == uuid).FirstOrDefaultAsync() == null)
                {
                    _db.DocSynches.Add(new DocSynch { Uuid = uuid });
                    await _db.SaveChangesAsync();
                    await next();
                }
                else
                    context.Result = new ContentResult { Content = null };
                */
            }
            await next();
            await _db.SaveChangesAsync();
        }
    }
}
