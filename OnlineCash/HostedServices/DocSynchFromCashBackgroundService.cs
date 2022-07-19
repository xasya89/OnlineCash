
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineCash.HostedServices
{
    public class DocSynchFromCashBackgroundService : BackgroundService
    {
        private readonly ILogger<DocSynchFromCashBackgroundService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public DocSynchFromCashBackgroundService(ILogger<DocSynchFromCashBackgroundService> logger, 
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
        }
    }
}
