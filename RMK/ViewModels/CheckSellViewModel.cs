using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace RMK.ViewModels
{
    public class CheckSellViewModel
    {
        public string CashierName { get; set; }
        public string CashierInn { get; set; }
        public bool isElectron { get; set; } = false;
        [JsonPropertyName("sum")]
        public decimal Sum { get; set; }
        [JsonPropertyName("sumDiscont")]
        public decimal SumDiscont { get; set; }
        [JsonPropertyName("sumAll")]
        public decimal SumAll { get; set; }
        [JsonPropertyName("goods")]
        public List<CheckGoodViewModel> Goods { get; set; } = new List<CheckGoodViewModel>();
    }
}
