using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public enum TypePayment
    {
        [Display(Name = "Электронные")]
        [Description("Электронные")]
        Electron=0,
        [Display(Name = "Наличные")]
        [Description("Электронные")]
        Cash =1
    }
}
