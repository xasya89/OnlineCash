﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class Writeof
    {
        public int Id { get; set; }
        public DateTime DateWriteof { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public string Note { get; set; }
        public decimal SumAll { get; set; }
        public bool IsSuccess { get; set; }
        public List<WriteofGood> WriteofGoods { get; set; } = new List<WriteofGood>();
    }
}
