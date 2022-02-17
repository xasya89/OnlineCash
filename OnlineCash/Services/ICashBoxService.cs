using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;
using OnlineCash.Models.CashBox;

namespace OnlineCash.Services
{
    public interface ICashBoxService
    {
        public Task<bool> OpenShift(int idShop, Guid uuid, DateTime start);
        public Task<Shift> CloseShift(Guid uuid, DateTime stop);
        public Task<bool> Buy(Guid uuid, List<CashBoxBuyReturnModel> buylist);
        public Task Sell(Guid uuidShop, CashBoxCheckSellModel check);
        public Task<bool> Return(int idShop, List<CashBoxBuyReturnModel> buylist);
    }
}
