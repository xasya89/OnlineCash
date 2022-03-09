using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class GoodBalanceHistory
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public DateTime CurDate { get; set; }
        public decimal CountLast { get; set; }
    }
}
