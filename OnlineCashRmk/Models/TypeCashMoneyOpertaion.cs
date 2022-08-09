using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public enum TypeCashMoneyOpertaion
    {
        [Display(Name = "Внесение")]
        Income=0,
        [Display(Name = "Изъятие")]
        Outcome=1
    }
}
