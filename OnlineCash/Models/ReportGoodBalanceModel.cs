using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Models
{
    public class ReportGoodBalanceModel
    {
        public DateTime CurDate { get; set; }
        public int ShopId { get; set; }
        public int? GoodGroupId { get; set; }
        public List<GoodBalanceHistory> GoodBalanceHistories { get; set; }
    }
}
