using OnlineCash.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Models;

namespace OnlineCash.Services
{
    public class ReportsService : IReportsService
    {
        shopContext db;
        public ReportsService(shopContext db)
        {
            this.db = db;
        }

        public async Task<ReportGoodBalanceModel> GetGoodBalanceHistory(DateTime curDay, int idShop, int? idGoodGroup)
        {
            curDay = curDay.Date;
            var balancesDb=await db.GoodBalanceHistories
                .Include(b => b.Good)
                .ThenInclude(g=>g.GoodPrices.Where(p=>p.ShopId==idShop))
                .Where(b => DateTime.Compare(b.CurDate, curDay) == 0 & b.ShopId == idShop)
                .OrderBy(b=>b.Good.Name)
                .ToListAsync();
            List<GoodBalanceHistory> balances = new List<GoodBalanceHistory>();
            foreach (var balance in balancesDb)
                if (balance.Good.IsDeleted==false && balance.Good.GoodPrices.Count(p => p.ShopId == idShop & p.BuySuccess==true) == 1)
                    if(idGoodGroup==null || balance.Good.GoodGroupId==idGoodGroup)
                        balances.Add(balance);
            return new ReportGoodBalanceModel
            {
                CurDate = curDay,
                ShopId = idShop,
                GoodGroupId = idGoodGroup,
                GoodBalanceHistories = balances
            };
        }
    }
}
