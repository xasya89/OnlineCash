using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Models;

namespace OnlineCash.Services
{
    public interface IWriteofService
    {
        public Task<bool> SaveSynch(int shopId, WriteofSynchModel writeof);
    }
}
