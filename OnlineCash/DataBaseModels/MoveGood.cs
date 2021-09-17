using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class MoveGood
    {
        public int Id { get; set; }
        public int MoveDocId { get; set; }
        public MoveDoc MoveDoc { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public double Count { get; set; }
        public decimal PriceConsigner { get; set; } //Отправитель
        public decimal PriceConsignee { get; set; } //Получатель
        public decimal SumPriceConsigner { get => (decimal)Count * SumPriceConsigner; }
        public decimal SumPriceConsignee { get => (decimal)Count * SumPriceConsignee; }
    }
}
