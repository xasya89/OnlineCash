﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public enum Units
    {
        [Display(Name ="шт")]
        PCE=796, //штука
        [Display(Name = "л")]
        Litr =112, //литр
        [Display(Name = "кг")]
        KG =166, //килограмм
        /*
        [Display(Name = "м")]
        MTR =6, //метр
        [Display(Name = "уп")]
        NMP =778, //упаковка
        [Display(Name = "см")]
        CMT =4 //сантиметр
        */
    }
}
