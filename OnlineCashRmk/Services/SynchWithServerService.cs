using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnlineCashRmk.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Services
{
    public class SynchWithServerService
    {
        private readonly ILogger<SynchWithServerService> _logger;
        private readonly IConfiguration _configuration;
        string serverurl;
        int shopId;
        int cronSynch;
        private readonly IServiceScopeFactory _scopeFactory;

        public SynchWithServerService(ILogger<SynchWithServerService> logger, 
            IConfiguration configuration, 
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            serverurl = _configuration.GetSection("serverName").Value;
            shopId = Convert.ToInt32(_configuration.GetSection("idShop").Value);
            cronSynch = Convert.ToInt32(_configuration.GetSection("Crons").GetSection("Synch").Value);
        }
        
        public async Task SendNow()
        {
            using (var scope = _scopeFactory.CreateScope())
            using (var db = scope.ServiceProvider.GetService<IDbContextFactory<DataContext>>().CreateDbContext())
            using (var httpClient = scope.ServiceProvider.GetService<IHttpClientFactory>().CreateClient())
            {

                try
                {
                    await SynchWithServerViewModel.StartSynch(serverurl, shopId, db, httpClient);
                }
                catch (HttpRequestException) { }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                }
            }
        }
    }

}
