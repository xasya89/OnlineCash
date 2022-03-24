using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public enum TypeDocs
    {
        [Description("Приход")]
        Arrival,
        [Description("Списание")]
        WriteOf,
        [Description("Инверторизация")]
        Stocktaking,
        [Description("Перемещение")]
        Move,
        [Description("Продажа")]
        Shift
    }
}
