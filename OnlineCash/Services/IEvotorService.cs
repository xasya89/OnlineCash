using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Services
{
    public interface IEvotorService
    {
        public Task<bool> ExchangeGoods();
    }
}
