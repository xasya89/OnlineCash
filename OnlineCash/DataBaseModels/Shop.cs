using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OrgName { get; set; }
        public string Inn { get; set; }
        public string Adress { get; set; }
        public List<GoodPrice> GoodPrices { get; set; }
        public List<Shift> Shifts { get; set; }
        public List<GoodBalance> GoodBalances { get; set; }
        public List<Arrival> Arrivals { get; set; }
        public List<Writeof> Writeofs { get; set; }
        public List<MoveDoc> MoveConsigneeDocs { get; set; }
        public List<MoveDoc> MoveConsignerDocs { get; set; }
        public List<GoodBalanceHistory> GoodBalanceHistories { get; set; }
        public List<SumBalanceHistory> SumBalanceHistories { get; set; }
    }
}
