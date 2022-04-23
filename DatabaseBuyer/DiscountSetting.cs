using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBuyer
{
    public class DiscountSetting
    {
        public int Id { get; set; }
        [Column(TypeName = "json")]
        public string Settings { get; set; }
    }
}
