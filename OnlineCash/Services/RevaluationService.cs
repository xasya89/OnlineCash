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
        public IConfiguration _configuration;
        public shopContext _db;
        public RevaluationService(shopContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
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
    }
}
