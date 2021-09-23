using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnlineCash.DataBaseModels
{
    public enum SpecialTypes
    {
        [Display(Name ="")]
        None,
        [Display(Name ="Пиво")]
        Beer,
        [Display(Name = "Тара")]
        Bottle,
        [Display(Name = "Пакет")]
        Bag
    }
}
