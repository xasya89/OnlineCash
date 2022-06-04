using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class ReportAfterStocktaking
    {
        public int Id { get; set; }
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime StopDate { get; set; }
        public int StocktakingId { get; set; }
        public Stocktaking Stocktaking { get; set; }
        public decimal StocktakingPrependSum { get; set; }
        public decimal CashStartSum { get; set; }
        public decimal SumDiscount { get; set; }
        public decimal IncomeSum { get; set; }
        public decimal ElectronSum { get; set; }
        public decimal ArrivalSum { get; set; }
        public decimal WriteOfSum { get; set; }
        public decimal RevaluationWriteOf { get; set; }
        public decimal RevaluationArrival { get; set; }
        public decimal CashEndSum { get; set; }

        public decimal StopSum { 
            get => StocktakingPrependSum + CashStartSum + ArrivalSum + RevaluationArrival - SumDiscount - RevaluationWriteOf - IncomeSum - ElectronSum - WriteOfSum - CashEndSum; 
        }

        public decimal StocktakingFactSum { get; set; }
    }
}
