using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OnlineCashRmk.Models
{
    public class StocktakingGood: INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int StocktakingGroupId { get; set; }
        public StocktakingGroup StocktakingGroup { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public Guid Uuid { get => Good.Uuid; }
        public string GoodName { get => Good?.Name; }
        public string UnitStr { get => Good?.Unit.DisplayName(); }
        [NotMapped]
        public string CountFactStr { 
            get => CountFact?.ToString();
            set
            {
                try
                {
                    CountFact = value.ToDecimal();
                }
                catch (NotFiniteNumberException)
                {
                    CountFact = null;
                }
                OnPropertyChanged(nameof(CountFact));
            }
        }
        public decimal? CountFact { get; set; }
        [JsonIgnore]
        public decimal? Price { get => Good?.Price; }
        [JsonIgnore]
        public decimal? Sum { get => Price * CountFact; }
        [JsonIgnore]
        public decimal? CountDocMove { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
