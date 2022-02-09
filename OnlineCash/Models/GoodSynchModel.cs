using DataBase;
using OnlineCash.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineCash.Models
{
    public class GoodSynchModel
    {
        [JsonPropertyName("uuid")]
        public Guid Uuid { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("article")]
        public string Article { get; set; }
        [JsonPropertyName("unit")]
        public Units Unit { get; set; }
        [JsonPropertyName("specialType")]
        public SpecialTypes SpecialType { get; set; } = SpecialTypes.None;
        [JsonPropertyName("vPackage")]
        public double? VPackage { get; set; }
        [JsonPropertyName("barcodes")]
        public List<string> Barcodes { get; set; } = new List<string>();
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
