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
        //public Task CalcGoodAsync(int ShopId, int GoodId);
        public Task PlusAsync(List<GoodBalanceModel> model);
        public Task PlusAsync(int ShopId, int GoodId, double Count);
        public Task MinusAsync(List<GoodBalanceModel> model);
        public Task MinusAsync(int ShopId, int GoodId, double Count);
        public Task ZerAsynco(Shop shop);
    }
}
