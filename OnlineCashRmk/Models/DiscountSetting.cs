using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineCashRmk.Models
{
    public class DiscountSetting
    {
        public int Id { get; set; }
        public string Discounts { get; set; }
        public DiscountParamContainerModel DiscountModel
        {
            get => JsonSerializer.Deserialize<DiscountParamContainerModel>(Discounts);
            set { Discounts = JsonSerializer.Serialize(value); }
        }
    }

    public class DiscountParamContainerModel 
    {
        public int Id { get; set; }
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
    public class DiscountParamModel
    {
        public int Id { get; set; }
        public Guid? Uuid { get; set; } = new Guid();
        public bool IsEnable { get; set; } = true;
    }
    public class DiscountParamBirthdayModel : DiscountParamModel
    {
        public int DayEnable { get; set; }
        public string TextSms { get; set; }
        public decimal? DiscountSum { get; set; }
        public int? DiscountPercent { get; set; }
    }
    public class DiscountParamHolidaysModel : DiscountParamModel
    {
        public DateTime DateHoliday { get; set; }
        public string TextSms { get; set; }
        public decimal? DiscountSum { get; set; }
        public int? DiscountPercent { get; set; }
    }
    public class DiscountParamNumBuyerModel : DiscountParamModel
    {
        public int NumBuyer { get; set; }
        public string TextSms { get; set; }
        public decimal? DiscountSum { get; set; }
        public int? DiscountPercent { get; set; }
    }
    public class DiscountParamSumBuyModel : DiscountParamModel
    {
        public decimal? SumBuyesMore { get; set; }
        public int? DiscountPercent { get; set; }
        public decimal? DiscountSum { get; set; }
    }
    public class DiscountParamSumOneBuyModel : DiscountParamModel
    {
        public decimal SumBuy { get; set; }
        public decimal? DiscountSum { get; set; }
        public int? DiscountPercent { get; set; }
    }
    public class DiscountParamWeeksModel
    {
        public int Id { get; set; }
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

    public class DiscountForSaleHowPercent
    {
        public decimal? Percent { get; set; }
    }
}
