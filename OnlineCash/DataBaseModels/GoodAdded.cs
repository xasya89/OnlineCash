using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCash.DataBaseModels
{
    public class GoodAdded
    {
        public int Id { get; set; }
        public int GoodId { get; set; }
        [JsonIgnore]
        public Good Good { get; set; }
    }
}
