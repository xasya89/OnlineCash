using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;

namespace OnlineCash.Services
{
    public interface IReportsService
    {
        public Task<ReportGoodBalanceModel> GetGoodBalanceHistory(DateTime curDay, int idShop, int? idGoodGroup);
    }
}
