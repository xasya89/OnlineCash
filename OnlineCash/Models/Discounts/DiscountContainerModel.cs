using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models.Discounts
{
    public class DiscountParamContainerModel
    {
        public List<DiscountParamSumBuyModel> SumBuys { get; set; } = new List<DiscountParamSumBuyModel>();
        public List<DiscountParamSumOneBuyModel> SumOneBuys { get; set; } = new List<DiscountParamSumOneBuyModel>();
        public List<DiscountParamNumBuyerModel> NumBuyer { get; set; } = new List<DiscountParamNumBuyerModel>();
        public List<DiscountParamHolidaysModel> Holidays { get; set; } = new List<DiscountParamHolidaysModel>();
        public List<DiscountParamBirthdayModel> Birthdays { get; set; } = new List<DiscountParamBirthdayModel>();
        public DiscountParamWeeksModel Weeks { get; set; } = new DiscountParamWeeksModel();
    }
}
