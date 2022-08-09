using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineCashRmk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;

namespace OnlineCashRmk.Services
{
    public class SynchService : ISynchService
    {
        DataContext db_;
        ILogger<SynchService> logger_;
        IConfiguration configuration;
        string hostname;
        int shopId;
        private static List<DocSynch> DocSynches = new List<DocSynch>();

        public SynchService(IDbContextFactory<DataContext> dbFactory, ILogger<SynchService> logger, IConfiguration configuration)
        {
            db_ = dbFactory.CreateDbContext();
            logger_ = logger;
            this.configuration = configuration;
            hostname = configuration.GetSection("serverName").Value;
            shopId = Convert.ToInt32(configuration.GetSection("idShop").Value);
        }
        public void AppendDoc(DocSynch docSynch)
        {
            db_.DocSynches.Add(docSynch);
            db_.SaveChanges();
            DocSynches.Add(docSynch);
        }

        public async Task<List<Supplier>> SynchSuppliersAsync()
        {
            var suppliers=await $"{hostname}/api/suppliers"
                .GetJsonAsync<List<Supplier>>();
            foreach(var supplier in suppliers)
            {
                var supplierDb = db_.Suppliers.Where(s => s.Id == supplier.Id).FirstOrDefault();
                if (supplierDb == null)
                    db_.Suppliers.Add(new Supplier { Id = supplier.Id, Name = supplier.Name, Inn = supplier.Inn, Kpp = supplier.Kpp });

            }
            db_.SaveChanges();
            return db_.Suppliers.ToList();
        }
    }
}
