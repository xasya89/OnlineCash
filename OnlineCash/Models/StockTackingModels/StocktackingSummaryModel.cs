using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase;

namespace OnlineCash.Models.StockTackingModels
{
    public class StocktackingSummaryModel
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public decimal CashMoneyDb { get; set; }
        public decimal CashMoneyFact { get; set; }
        public decimal SumDb { get; set; }
        public decimal SumFact { get; set; }
        public List<StocktackingSummaryGoodModel> Goods { get; set; } = new List<StocktackingSummaryGoodModel>();
    }
    public class StocktackingSummaryGoodModel
    {
        public int GoodId { get; set; }
        public string GoodName { get; set; }
        public Units Unit { get; set; }
        public decimal Price { get; set; }
        public decimal CountLast { get; set; }
        public decimal CountDb { get; set; }
        public decimal CountFact { get; set; }
        public decimal CountEnd { get => CountDb - CountFact; }
        public decimal SumEnd { get => CountEnd * Price; }
    }
}
