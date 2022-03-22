using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models;
using OnlineCash.Services;

namespace OnlineCash.Controllers.Api.OnlineCash
{
    [Route("api/onlinecash/[controller]")]
    [ApiController]
    public class RevaluationSynchController : ControllerBase
    {
        private readonly RevaluationService _service;
        NotificationOfEventInSystemService _notificationService;
        public RevaluationSynchController(RevaluationService service, NotificationOfEventInSystemService notificationService)
        {
            _service = service;
            _notificationService = notificationService;
        }

        [HttpPost("{shopId}")]
        public async Task<RevaluationModel> GetSynch(int shopId, [FromBody] RevaluationModel model)
        {
            RevaluationModel rModel= await _service.SaveSynch(model);
            await _notificationService.Send($@"Переоценка 
Предыдущая стоимость - {rModel.RevaluationGoods.Sum(r=>r.Count * r.PriceOld)}
Новая стоимость - {rModel.RevaluationGoods.Sum(r=>r.Count * r.PriceNew)}", "Revaluation/Edit/" + rModel.Id);
            return rModel;
        }
    }
}
