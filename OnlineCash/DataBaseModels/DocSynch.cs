using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash.DataBaseModels
{
    public class DocSynch
    {
        public int Id { get; set; }
        public DateTime DateAppend { get; set; } = DateTime.Now;
        public Guid Uuid { get; set; }
    }
}
