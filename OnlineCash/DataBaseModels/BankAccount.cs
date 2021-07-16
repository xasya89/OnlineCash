using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineCash.DataBaseModels
{
    public class BankAccount
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("alias")]
        [Required(ErrorMessage = "Не заполнено поле Псевдоним")]
        public string Alias { get; set; }
        [JsonPropertyName("bankName")]
        public string BankName { get; set; }
        [JsonPropertyName("num")]
        public string Num { get; set; }
        [JsonPropertyName("korShet")]
        public string KorShet { get; set; }
        [JsonPropertyName("bik")]
        public string Bik { get; set; }
        [JsonPropertyName("arrivalPayments")]
        [JsonIgnore]
        public List<ArrivalPayment> ArrivalPayments { get; set; }
    }
}
