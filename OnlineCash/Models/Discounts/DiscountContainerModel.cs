using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCash.Models.Discounts
{
    public class DiscountParamContainerModel
    {
        [JsonPropertyName("sumBuys")]
        public List<DiscountParamSumBuyModel> SumBuys { get; set; } = new List<DiscountParamSumBuyModel>();
        [JsonPropertyName("sumOneBuys")]
        public List<DiscountParamSumOneBuyModel> SumOneBuys { get; set; } = new List<DiscountParamSumOneBuyModel>();
        [JsonPropertyName("numBuyer")]
        public List<DiscountParamNumBuyerModel> NumBuyer { get; set; } = new List<DiscountParamNumBuyerModel>();
        [JsonPropertyName("holidays")]
        public List<DiscountParamHolidaysModel> Holidays { get; set; } = new List<DiscountParamHolidaysModel>();
        [JsonPropertyName("birthdays")]
        public List<DiscountParamBirthdayModel> Birthdays { get; set; } = new List<DiscountParamBirthdayModel>();
        [JsonPropertyName("weeks")]
        public List<DiscountParamWeeksModel> Weeks { get; set; }
        //public Dictionary<int, DiscountParamWeeksModel> Weeks { get; set; } = new Dictionary<int, DiscountParamWeeksModel>();
        //public DiscountParamWeeksModel Weeks { get; set; } = new DiscountParamWeeksModel();
    }
}
