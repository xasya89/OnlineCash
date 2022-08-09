using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class DocSynch
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public TypeDocs TypeDoc { get;set;  }
        public int DocId { get; set; }
        public DateTime Create { get; set; } = DateTime.Now;
        public bool SynchStatus { get; set; } = false;
        public DateTime? Synch { get; set; }

        public override string ToString() => $"{TypeDoc.GetDisplay()} {Create.ToString("dd.MM HH:mm")}";
    }
}
