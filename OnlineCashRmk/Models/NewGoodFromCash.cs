using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class NewGoodFromCash
    {
        public int Id { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
    }
}
