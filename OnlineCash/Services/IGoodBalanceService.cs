using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;

namespace OnlineCash.Services
{
    public interface IGoodBalanceService
    {
        public Task<bool> CalcAsync(int ShopId, DateTime curDate);
        public Task PlusAsync(List<GoodBalanceModel> model);
        public Task MinusAsync(List<GoodBalanceModel> model);
        public Task ZerAsynco(Shop shop);
    }
}
