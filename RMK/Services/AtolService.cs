using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atol.Drivers10.Fptr;
using RMK.ViewModels;

namespace RMK.Services
{
    public class AtolService:IAtolService
    {
        IFptr kkt;
        public AtolService()
        {
            try
            {
                kkt = new Fptr();
            }
            catch(SystemException ex) { }
        }

        public void ToggleSession()
        {
            if (kkt.isOpened())
                kkt.close();
            else
                kkt.open();
        }

        public string GetVersion()
        {
            return kkt.version();
        }

        public void OpenShift(string CashierName, string CashierInn)
        {
            if (!kkt.isOpened())
                kkt.open();
            kkt.setParam(1021, CashierName);
            kkt.setParam(1203, CashierInn);
            kkt.operatorLogin();
            kkt.openShift();
            kkt.checkDocumentClosed();
        }

        public void CloseShift()
        {
            if (!kkt.isOpened())
                kkt.open();
            kkt.setParam(Constants.LIBFPTR_PARAM_REPORT_TYPE, Constants.LIBFPTR_RT_CLOSE_SHIFT);
            kkt.report();

            kkt.checkDocumentClosed();
        }

        public void CashIncome(decimal sum)
        {
            if (!kkt.isOpened())
                kkt.open();
            kkt.setParam(Constants.LIBFPTR_PARAM_SUM, Convert.ToDouble(sum));
            kkt.cashIncome();
        }

        public void CashOutcome(decimal sum)
        {
            if (!kkt.isOpened())
                kkt.open();
            kkt.setParam(Constants.LIBFPTR_PARAM_SUM, Convert.ToDouble(sum));
            kkt.cashIncome();
        }

        public bool CheckSell(CheckSellViewModel checkSell)
        {
            if (!kkt.isOpened())
                return false;
            kkt.setParam(1021, checkSell.CashierName);
            kkt.setParam(1203, checkSell.CashierInn);
            kkt.operatorLogin();

            kkt.setParam(Constants.LIBFPTR_PARAM_RECEIPT_TYPE, Constants.LIBFPTR_RT_SELL);
            if (checkSell.isElectron)
                kkt.setParam(Constants.LIBFPTR_PARAM_RECEIPT_ELECTRONICALLY, true);
            kkt.openReceipt();
            foreach (var good in checkSell.Goods)
            {
                kkt.setParam(Constants.LIBFPTR_PARAM_COMMODITY_NAME, good.Name);
                kkt.setParam(Constants.LIBFPTR_PARAM_PRICE, Convert.ToDouble(good.Price));
                kkt.setParam(Constants.LIBFPTR_PARAM_QUANTITY, Convert.ToDouble(good.Count));
                kkt.setParam(Constants.LIBFPTR_PARAM_TAX_TYPE, Constants.LIBFPTR_TAX_NO);
                kkt.registration();
            }
            kkt.setParam(Constants.LIBFPTR_PARAM_PAYMENT_TYPE, Constants.LIBFPTR_PT_CASH);
            kkt.setParam(Constants.LIBFPTR_PARAM_PAYMENT_SUM, Convert.ToDouble(checkSell.SumAll));
            kkt.payment();
            kkt.closeReceipt();
            return true;
        }
    }
}
