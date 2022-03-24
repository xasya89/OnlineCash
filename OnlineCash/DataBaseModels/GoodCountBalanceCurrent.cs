using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    [Index(nameof(GoodId), IsUnique = true)]
    public class GoodCountBalanceCurrent
    {
        public int Id { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public decimal Count { get; set; }
    }
}
