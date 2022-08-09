using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public enum TypeDocs
    {
        //0
        [Description("Открытие смены")]
        [Display(Name = "Открытие смены")]
        OpenShift,
        //1
        [Description("Покупка")]
        [Display(Name = "Покупка")]
        Buy,
        //2
        [Description("Возврат")]
        [Display(Name = "Возврат")]
        Return,
        //3
        [Description("Закрытие смены")]
        [Display(Name = "Закрытие смены")]
        CloseShift,
        //4
        [Description("Списание")]
        [Display(Name = "Списание")]
        WriteOf,
        //5
        [Description("Приход")]
        [Display(Name = "Приход")]
        Arrival,
        //6
        [Description("Инверторизация")]
        [Display(Name = "Инверторизация")]
        StockTaking,
        //7
        [Description("Деньги в кассе")]
        [Display(Name = "Деньги в кассе")]
        CashMoney,
        //8
        [Description("Новый товар")]
        [Display(Name = "Новый товар")]
        NewGoodFromCash,
        //9
        [Description("Переоценка")]
        [Display(Name ="Переоценка")]
        Revaluation,
        //10
        [Description("Начало инверторизации")]
        [Display(Name = "Начало инверторизации")]
        StartStocktacking,
        //11
        [Description("Окончание инверторизации")]
        [Display(Name = "Окончание инверторизации")]
        StopStocktacking
    }
}
