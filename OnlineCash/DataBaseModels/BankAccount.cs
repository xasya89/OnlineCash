using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OnlineCash.DataBaseModels
{
    public class BankAccount
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Не заполнено поле Псевдоним")]
        public string Alias { get; set; }
        public string BankName { get; set; }
        public string Num { get; set; }
        public string KorShet { get; set; }
        public string Bik { get; set; }
        public List<ArrivalPayment> ArrivalPayments { get; set; }
    }
}
