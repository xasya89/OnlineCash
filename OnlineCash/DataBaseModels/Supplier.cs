using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace OnlineCash.DataBaseModels
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Inn { get; set; }
        public List<Arrival> Arrivals { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public List<Good> Goods { get; set; }
    }
}
