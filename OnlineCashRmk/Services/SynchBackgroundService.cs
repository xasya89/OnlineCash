using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;
using OnlineCashRmk.DataModels;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using System.Threading;
using OnlineCashRmk.ViewModels;
using Microsoft.Extensions.Logging;


namespace OnlineCashRmk.Services
{
    class SynchBackgroundService : BackgroundService
    {
        private readonly ILogger<SynchBackgroundService> _logger;
        private readonly DataContext db;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        string serverurl;
        int shopId;
        int cronSynch;
        private readonly IServiceScopeFactory _scopeFactory;

        public SynchBackgroundService(
            ILogger<SynchBackgroundService> logger,
            IServiceScopeFactory scopeFactory, 
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            serverurl = _configuration.GetSection("serverName").Value;
            shopId = Convert.ToInt32(_configuration.GetSection("idShop").Value);
            cronSynch = Convert.ToInt32(_configuration.GetSection("Crons").GetSection("Synch").Value);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                using (var scope = _scopeFactory.CreateScope())
                using (var db = scope.ServiceProvider.GetService<IDbContextFactory<DataContext>>().CreateDbContext())
                using (var httpClient = _httpClientFactory.CreateClient())
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
                    await Task.Delay(TimeSpan.FromMinutes(cronSynch));
                }

        }



    }

    public class CashBoxCheckSellBuyer
    {
        public Guid Uuid { get; set; }
        public string Phone { get; set; }
    }

    public class CashBoxCheckSellGood
    {
        public Guid Uuid { get; set; }
        public decimal Count { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
    }

    public class CashBoxCheckSellModel
    {
        public DateTime Create { get; set; }
        public bool IsReturn { get; set; } = false;
        public CashBoxCheckSellBuyer Buyer { get; set; }
        public decimal SumCash { get; set; }
        public decimal SumElectron { get; set; }
        public decimal SumDiscount { get; set; }
        public List<CashBoxCheckSellGood> Goods { get; set; } = new List<CashBoxCheckSellGood>();
    }
}
