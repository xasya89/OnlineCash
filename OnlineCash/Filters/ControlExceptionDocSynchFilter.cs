using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Services;

namespace OnlineCash.Filters
{
    public class ControlExceptionDocSynchFilter:Attribute, IAsyncExceptionFilter
    {
        shopContext _db;
        public ControlExceptionDocSynchFilter(shopContext db)
            => _db = db;

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            
            string actionName = context.ActionDescriptor.DisplayName;
            string exceptionStack = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;
            string uuidStr = context.HttpContext.Request.Headers.Where(h => h.Key.ToLower() == "doc-uuid").Select(h => h.Value).FirstOrDefault();
            Guid uuid = Guid.Parse(uuidStr);
            var docSync = await _db.DocSynches.Where(d => d.Uuid == uuid).FirstOrDefaultAsync();
            if(docSync!=null)
            {
                _db.DocSynches.Remove(docSync);
                await _db.SaveChangesAsync();
            }
            
            context.Result = new ContentResult
            {
                Content = $"В методе {actionName} возникло исключение: \n {exceptionMessage} \n {exceptionStack}",
                 StatusCode=500
            };
            context.ExceptionHandled = true;
        }
    }
}
