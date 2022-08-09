using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCashRmk.Models;

namespace OnlineCashRmk.Services
{
    class GoodChangeOrNewService
    {
        DataContext _db;
        public GoodChangeOrNewService(DataContext db)
        {
            _db = db;
        }
        public async Task Change(Good good)
        {

        }
    }
}
