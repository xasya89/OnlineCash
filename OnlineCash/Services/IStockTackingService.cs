using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Models.StockTackingModels;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;

namespace OnlineCash.Services
{
    public interface IStockTackingService
    {
        public Task<StockTackingGruppingModel> GetDetailsGroups(int idStocktaking);
        public Task StartFromOnlineCash(int shopId, StocktakingReciveDataModel model);
        public Task StopFromOnlineCash(Guid uuid, List<StocktakingGroupReciveDataModel> groups);
        public Task SaveFromOnlinCash(int shopId, StocktakingReciveDataModel model);
        public Task Save(StockTakingSaveModel model);
        public Task<StocktackingSummaryModel> GetSummary(int id, int? goodGroupdId);
        public Task ChangeSummary(int stocktackingId, List<StocktackingSummaryGoodModel> model);

        public Task<List<dynamic>> GetDetailGoodCountByDocs(int stocktakingId, int goodId);
    }
}
