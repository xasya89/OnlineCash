using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models;

namespace OnlineCash.Services
{
    public interface ICashBoxService
    {
        public Task<bool> OpenShift(int idShop);
        public Task<bool> CloseShift(int idShop);
        public Task<bool> Buy(int idShop, List<CashBoxBuyReturnModel> buylist);
        public Task<bool> Return(int idShop, List<CashBoxBuyReturnModel> buylist);
    }
}
