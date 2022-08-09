using OnlineCash.DataBaseModels;
using System;

namespace OnlineCash.Models
{
    public class GoodBalanceSynchModel
    {
        public int DocumentId { get; set; }
        public DateTime DocumentDate { get; set; }
        public TypeDocs TypeDoc { get; set; }
        public int GoodId { get; set; }
        public decimal Count { get; set; }
    }
}
