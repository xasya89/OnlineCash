﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCash.Models.BuyerRegister
{
    public class BuyerRegistrationModel
    {
        [JsonPropertyName("uuid")]
        public Guid? Uuid { get; set; }
        [JsonPropertyName("cardNum")]
        public string CardNum { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
