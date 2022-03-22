using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public enum DocumentStatus
    {
        [Description("")]
        [Display(Name = "")]
        New=0,
        [Description("")]
        [Display(Name = "")]
        Edit=1,
        [Description("Подтвержден")]
        [Display(Name = "Подтвержден")]
        Confirm=2,
        [Description("Удален")]
        [Display(Name = "Удален")]
        Remove=3
    }
}
