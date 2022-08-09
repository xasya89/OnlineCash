using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class Writeof
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public DateTime DateCreate { get; set; }
        public string Note { get; set; }
        public decimal SumAll { get; set; }
        public List<WriteofGood> WriteofGoods { get; set; } = new List<WriteofGood>();
    }
}
