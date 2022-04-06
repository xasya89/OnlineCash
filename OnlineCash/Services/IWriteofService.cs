using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models;
using OnlineCash.DataBaseModels;

namespace OnlineCash.Services
{
    public interface IWriteofService
    {
        public Task<Writeof> SaveSynch(int shopId, WriteofSynchModel writeof);
        public Task Create(Writeof model);
        public Task Edit(Writeof model);
    }
}
