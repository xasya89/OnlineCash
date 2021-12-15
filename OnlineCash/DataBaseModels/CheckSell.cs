﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class CheckSell
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public int? BuyerId { get; set; }
        public Buyer Buyer { get; set; }
        public bool IsElectron { get; set; }
        public decimal Sum { get; set; }
        public decimal SumDiscont { get; set; } = 0;
        public decimal SumAll { get; set; }
        public List<CheckGood> CheckGoods { get; set; } = new List<CheckGood>();
        public int ShiftId { get; set; }
        public Shift Shift { get; set; }
    }
}
