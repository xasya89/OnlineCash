using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCashRmk.Models;

namespace OnlineCashRmk.DataModels
{
    public class CheckPaymentDataModel
    {
        public TypePayment TypePayment { get; set; }
        public string TypePaymentStr
        {
            get => TypePayment.GetDisplay();
        }
        public decimal Income { get; set; }
        public decimal Sum { get; set; }
        public decimal Return
        {
            get
            {
                if (TypePayment == TypePayment.Electron) return 0;
                if (TypePayment == TypePayment.Cash)
                    return Income - Sum;
                return 0;
            }
        }
    }
}
