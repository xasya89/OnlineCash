using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCash.Models.Discounts
{
    public class DiscountParamWeeksModel
    {
        [JsonPropertyName("dayNum")]
        public int DayNum { get; set; }
        [JsonPropertyName("dayName")]
        public string DayName { get; set; }
        [JsonPropertyName("timeWith")]
        public string? TimeWith { get; set; }
        [JsonPropertyName("timeBy")]
        public string? TimeBy { get; set; }
        [JsonPropertyName("discountPercent")]
        public int? DiscountPercent { get; set; }
    }
}
