using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;
using Microsoft.Extensions.Configuration;

namespace OnlineCash.Services
{
    public class RevaluationService
    {
        private readonly IConfiguration _configuration;
        private readonly shopContext _db;
        private readonly NotificationOfEventInSystemService _notification;
        public RevaluationService(shopContext db, IConfiguration configuration, NotificationOfEventInSystemService notification)
        {
            _db = db;
            _configuration = configuration;
            _notification = notification;
        }

        public async Task<RevaluationModel> SaveSynch(RevaluationModel model)
        {
            var revaluationDb = await _db.Revaluations.Where(r => r.Uuid == model.Uuid).FirstOrDefaultAsync();
            if (revaluationDb != null)
                throw new Exception($"Документ с пересортица с uuid - {model.Uuid} уже существует в базе данных");

            var goods = await _db.Goods.ToListAsync();
            var balanseCurrents = await _db.GoodCountBalanceCurrents.ToListAsync();
            foreach (var rGood in model.RevaluationGoods)
                if (goods.Where(g => g.Uuid == rGood.Uuid).FirstOrDefault() == null)
                    throw new Exception($"При попытке сохранения акта Переоценки, товар с uuid - {rGood.Uuid} не найден");
            bool autosave = _configuration.GetSection("AutoSuccessFromCash").Value == "1";
            Revaluation revaluation = new Revaluation { Status=autosave ? DocumentStatus.Confirm : DocumentStatus.New, Create=model.Create};
            List<RevaluationGood> revaluationGoods = new List<RevaluationGood>();
            foreach(var rGood in model.RevaluationGoods)
            {
                int goodId = goods.Where(g => g.Uuid == rGood.Uuid).FirstOrDefault().Id;
                decimal count = (decimal?)balanseCurrents.Where(b => b.GoodId == goodId).FirstOrDefault()?.Count ?? 0;
                RevaluationGood revaluationGood = new RevaluationGood
                {
                    Revaluation = revaluation,
                    GoodId = goodId,
                    Count = count,
                    PriceOld = rGood.PriceOld,
                    PriceNew = rGood.PriceNew
                };
                revaluationGoods.Add(revaluationGood);
                rGood.Count = count;
            };
            revaluation.SumOld = revaluationGoods.Sum(r => r.Count * r.PriceOld);
            revaluation.SumNew = revaluationGoods.Sum(r => r.Count * r.PriceNew);
            _db.Revaluations.Add(revaluation);
            _db.RevaluationGoods.AddRange(revaluationGoods);
            await _db.SaveChangesAsync();
            model.Id = revaluation.Id;
            return model;
        }

        public async Task<List<Revaluation>> GetRevaluations()
            => await _db.Revaluations.OrderByDescending(r => r.Create).ToListAsync();

        public async Task<Revaluation> GetRevaluation(int id)
            => await _db.Revaluations.Include(r => r.RevaluationGoods).ThenInclude(r => r.Good).Where(r => r.Id == id).FirstOrDefaultAsync();

        public async Task Create(List<RevaluationAddModel> goods)
        {
            var revaluation = await _db.Revaluations.Where(r => DateTime.Compare(r.Create.Date, DateTime.Now) == 0).FirstOrDefaultAsync();
            if (revaluation == null)
            {
                revaluation = new Revaluation { Create = DateTime.Now, Status = DocumentStatus.Confirm };
                _db.Revaluations.Add(revaluation);
            };
            string notifyMessage = "Переоценка";
            foreach (var good in goods)
            {
                decimal countCurrent = (await _db.GoodCountBalanceCurrents.Where(g => g.GoodId == good.Good.Id).FirstOrDefaultAsync())?.Count ?? 0;
                _db.RevaluationGoods.Add(new RevaluationGood
                {
                    Revaluation = revaluation,
                    GoodId = good.Good.Id,
                    PriceOld = good.PriceOld,
                    PriceNew = good.PriceNew,
                    Count = countCurrent
                });
                revaluation.SumOld += good.PriceOld * countCurrent;
                revaluation.SumNew += good.PriceNew * countCurrent;
                notifyMessage += $"\n{good.Good.Name} было {good.PriceOld * countCurrent} стало {good.PriceNew * countCurrent}\nКоличество {countCurrent}";
            };
            await _db.SaveChangesAsync();
            await _notification.Send(notifyMessage);
        }
    }
}
