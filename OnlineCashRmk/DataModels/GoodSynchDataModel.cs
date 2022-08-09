using OnlineCashRmk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCashRmk.DataModels
{
    class GoodSynchDataModel
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
        public IEnumerable<string> Barcodes { get; set; } = new List<string>();
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
