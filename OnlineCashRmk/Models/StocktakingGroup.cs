using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class StocktakingGroup
    {
        public int Id { get; set; }
        public int StocktakingId { get; set; }
        public Stocktaking Stocktaking { get; set; }
        public string Name { get; set; }
        public List<StocktakingGood> StocktakingGoods { get; set; } = new List<StocktakingGood>();
        [NotMapped]
        public decimal? Sum { get => StocktakingGoods.Sum(g => g.CountFact * g.Price); }

        public override string ToString() => $"{Name} - {Sum} р.";
    }
}
