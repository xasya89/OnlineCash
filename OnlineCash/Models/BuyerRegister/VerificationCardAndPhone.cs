using System;
using System.Text.Json.Serialization;

namespace OnlineCash.Models.BuyerRegister
{
    public class VerificationCardAndPhone
    {
        [JsonPropertyName("uuid")]
        public Guid? Uuid { get; set; }
        [JsonPropertyName("cardNum")]
        public string CardNum { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
