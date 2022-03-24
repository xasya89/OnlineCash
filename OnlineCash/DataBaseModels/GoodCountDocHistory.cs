using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class GoodCountDocHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public int DocId { get; set; }
        public TypeDocs TypeDoc { get; set; }
        public decimal Count { get; set; }
    }
}
