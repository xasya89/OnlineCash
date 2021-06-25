using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace RMK.ViewModels
{
    public class CheckGoodViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("count")]
        public double Count { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
