using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class GoodPrice
    {
        public int Id { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public decimal Price { get; set; }
    }
}
