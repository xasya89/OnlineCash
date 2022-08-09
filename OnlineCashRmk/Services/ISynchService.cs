using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCashRmk.Models;

namespace OnlineCashRmk.Services
{
    public interface ISynchService
    {
        public void AppendDoc(DocSynch docSynch);
        public Task<List<Supplier>> SynchSuppliersAsync();
    }
}
