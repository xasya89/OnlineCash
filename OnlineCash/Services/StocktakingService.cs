using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models;

namespace OnlineCash.Services
{
    public class StocktakingService
    {
        shopContext _db;
        public StocktakingService(shopContext db)
        {
            _db = db;
        }

        public async Task SaveFromOnlinCash(StockTakingSaveModel model)
        {

        }
    }
}
