using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class RevaluationModel
    {
        public int Id { get; set; }
        public DateTime Create { get; set; }
        public Guid Uuid { get; set; }
        public List<RevaluationGoodModel> RevaluationGoods { get; set; } = new List<RevaluationGoodModel>();
    }

    public class RevaluationGoodModel
    {
        public Guid Uuid { get; set; }
        public decimal? Count { get; set; }
        public decimal PriceOld { get; set; }
        public decimal PriceNew { get; set; }
    }
}
