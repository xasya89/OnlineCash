using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OnlineCashRmk.Models;

namespace OnlineCashRmk.DataModels
{
    public class ArrivalPositionDataModel : INotifyPropertyChanged
    {
        public int GoodId { get; set; }

        public string GoodName { get; set; }

        public Units Unit { get; set; }
        public string UnitStr
        {
            get => Unit.GetDisplay();
        }

        private decimal priceArrival;
        public decimal PriceArrival
        {
            get => priceArrival;
            set { priceArrival = value; NotifyPropertyChanged(nameof(PriceArrival)); }
        }

        private decimal priceSell;
        public decimal PriceSell
        {
            get => priceSell;
            set { priceSell = value; NotifyPropertyChanged(nameof(PriceSell)); }
        }

        public decimal? PricePercent
        {
            get => PriceArrival == 0 ? null : Math.Round(PriceSell / PriceArrival * 100, 2);
        }

        private decimal count;
        public decimal Count
        {
            get => count;
            set { count = value; NotifyPropertyChanged(nameof(Count)); }
        }

        public decimal SumArrival
        {
            get
            {
                decimal sum=PriceArrival * (decimal)Count;
                switch (NdsPercent)
                {
                    case "20%":
                        //return SumArrival / 120 * 20;
                        return sum+ sum * 0.2M;
                        break;
                    case "10%":
                        //return SumArrival / 110 * 10;
                        return sum+sum * 0.1M;
                        break;
                    case "0%":
                        return sum;
                        break;
                    default:
                        return sum;
                        break;
                }
            }
        }

        private string ndsPercent="Без ндс";
        public string NdsPercent
        {
            get => ndsPercent;
            set
            {
                ndsPercent = value;
                NotifyPropertyChanged(nameof(ndsPercent));
            }
        }

        public decimal SumNds
        {
            get
            {
                decimal sum = PriceArrival * (decimal)Count;
                switch (NdsPercent)
                {
                    case "20%":
                        //return SumArrival / 120 * 20;
                        return sum * 0.2M;
                        break;
                    case "10%":
                        //return SumArrival / 110 * 10;
                        return sum * 0.1M;
                        break;
                    case "0%":
                        return 0;
                        break;
                    default:
                        return 0;
                        break;
                }
            }
        }
        public string SumNdsStr => SumNds.ToSellFormat();

        public decimal SumSell
        {
            get => PriceSell * (decimal)Count;
        }

        public DateTime? ExpiresDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") =>
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(propertyName)
                    );
    }
}
