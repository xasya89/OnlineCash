using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Services
{
    public interface IReportsService
    {
        public Task<List<GoodBalanceHistory>> GetGoodBalanceHistory(DateTime curDay, int idShop);
    }
}
