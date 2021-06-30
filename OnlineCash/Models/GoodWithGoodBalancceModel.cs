using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Models
{
    public class GoodWithGoodBalanceModel
    {
        public Good Good { get; set; }
        public double? CountOnBalance { get; set; }
    }
}
