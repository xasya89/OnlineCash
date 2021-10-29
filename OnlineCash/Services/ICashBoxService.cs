using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models;

namespace OnlineCash.Services
{
    public interface ICashBoxService
    {
        public Task<bool> OpenShift(int idShop, Guid uuid, DateTime start);
        public Task<bool> CloseShift(Guid uuid, DateTime stop);
        public Task<bool> Buy(Guid uuid, List<CashBoxBuyReturnModel> buylist);
        public Task<bool> Return(int idShop, List<CashBoxBuyReturnModel> buylist);
    }
}
