using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.DataModels
{
    public class RevaluationDataModel
    {
        public DateTime Create { get; set; }
        public Guid Uuid { get; set; }
        public List<RevaluationGoodDataModel> RevaluationGoods { get; set; } = new List<RevaluationGoodDataModel>();
    }

    public class RevaluationGoodDataModel
    {
        public Guid Uuid { get; set; }
        public decimal? Count { get; set; }
        public decimal PriceOld { get; set; }
        public decimal? PriceNew { get; set; }
    }
}
