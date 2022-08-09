using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk
{
    public static class DecimalHelper
    {
        public static decimal ToDecimal( this string str)
        {
            decimal dec = -1;
            if (!decimal.TryParse(str.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out dec))
                return 0;
            //TODO: стоит ли использовать исключение при конвертиации из string to Decimal
                //throw new NotFiniteNumberException("Ошибка преобразования строки в decimal");
            return dec;
        }

        public static double ToDouble(this string str)
        {
            double dec = -1;
            if (!double.TryParse(str.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out dec))
                throw new NotFiniteNumberException("Ошибка преобразования строки в decimal");
            return dec;
        }

        public static string ToSellFormat(this decimal val)
            => String.Format("{0:0.00}", val);

        public static string ToCountFormat(this decimal val)
            => String.Format("{0:0.000}", val);
    }
}
