using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCash.Models.Discounts
{
    public class DiscountParamContainerModel
    {
        [JsonPropertyName("percentFromSale")]
        public decimal? PercentFromSale { get; set; }
        [JsonPropertyName("sumBuys")]
        public List<DiscountParamSumBuyModel> SumBuys { get; set; } = new();
        [JsonPropertyName("sumOneBuys")]
        public List<DiscountParamSumOneBuyModel> SumOneBuys { get; set; } = new();
        [JsonPropertyName("numBuyer")]
        public List<DiscountParamNumBuyerModel> NumBuyer { get; set; } = new();
        [JsonPropertyName("holidays")]
        public List<DiscountParamHolidaysModel> Holidays { get; set; } = new();
        [JsonPropertyName("birthdays")]
        public List<DiscountParamBirthdayModel> Birthdays { get; set; } = new();
        [JsonPropertyName("weeks")]
        public List<DiscountParamWeeksModel> Weeks { get; set; }
        //public Dictionary<int, DiscountParamWeeksModel> Weeks { get; set; } = new Dictionary<int, DiscountParamWeeksModel>();
        //public DiscountParamWeeksModel Weeks { get; set; } = new DiscountParamWeeksModel();
    }

    public class DiscountForSaleHowPercent
    {
        public decimal? Percent { get; set; }
    }
}
