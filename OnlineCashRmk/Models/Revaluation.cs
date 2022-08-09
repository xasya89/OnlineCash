using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class Revaluation
    {
        public int Id { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Create { get; set; } = DateTime.Now;
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public List<RevaluationGood> RevaluationGoods { get; set; } = new List<RevaluationGood>();
    }

    public class RevaluationGood
    {
        public int Id { get; set; }
        public int RevaluationId { get; set; }
        public Revaluation Revaluation { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public decimal? Count { get; set; }
        public decimal PriceOld { get; set; }
        public decimal SumOld { get => (Count ?? 0) * PriceOld; }
        public decimal PriceNew { get; set; }
        public decimal SumNew { get => (Count ?? 0) * PriceOld; }
    }
}
