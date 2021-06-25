using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMK.ViewModels;

namespace RMK.Services
{
    public interface IAtolService
    {
        public string GetVersion();
        public void ToggleSession();
        public void OpenShift(string CashierName, string CahierInn);
        public void CloseShift();
        public void CashIncome(decimal sum);
        public void CashOutcome(decimal sum);
        public bool CheckSell(CheckSellViewModel checkSell);
    }
}
