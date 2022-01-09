﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models.Discounts
{
    public class DiscountParamNumBuyerModel:DiscountParamModel
    {
        public int NumBuyer { get; set; }
        public string TextSms { get; set; }
        public decimal? DiscountSum { get; set; }
        public int? DiscountPercent { get; set; }
    }
}