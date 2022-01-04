using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Models.StockTackingModels;

namespace OnlineCash.Services
{
    public interface IStockTackingService
    {
        public Task<StockTackingGruppingModel> GetDetailsGroups(int idStocktaking);
        public Task SaveFromOnlinCash(int shopId, StocktakingReciveDataModel model);
    }
}
