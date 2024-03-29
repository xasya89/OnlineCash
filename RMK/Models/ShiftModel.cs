﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMK.Models
{
    public class ShiftModel
    {
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public decimal SumAll { get; set; }
        public decimal SumSell { get; set; }
        public decimal SummReturn { get; set; }
        public decimal SumIncome { get; set; }
        public decimal SumOutcome { get; set; }
        public List<CheckSellModel> CheckSells { get; set; }
    }
}
