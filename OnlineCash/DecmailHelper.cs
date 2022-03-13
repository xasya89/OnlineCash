using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash
{
    public static class DecmailHelper
    {
        public static string CountToString(this decimal value)
        {
            string strValue = value.ToString(); //Get the stock string
            //If there is a decimal point present
            if (strValue.Contains("."))
            {
                //Remove all trailing zeros
                strValue = strValue.TrimEnd('0');

                //If all we are left with is a decimal point
                if (strValue.EndsWith(".")) //then remove it
                    strValue = strValue.TrimEnd('.');
            }

            return strValue;
        }
        public static decimal Normalize(this decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }
    }
}
