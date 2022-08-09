using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace OnlineCashRmk.Services
{
    public class SynchRabbitBackgroundService: BackgroundService
    {
        IServiceScopeFactory _scopeFactory;
        ILogger<SynchRabbitBackgroundService> _logger;
        IConfiguration _configuration;
        public SynchRabbitBackgroundService(IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<SynchRabbitBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = _scopeFactory.CreateScope();
            var dbFactory = scope.ServiceProvider.GetService<IDbContextFactory<DataContext>>();
            using var db = dbFactory.CreateDbContext();
            try
            {
                var factory = new ConnectionFactory() { HostName = "soft-impex.ru", UserName = "anikulshin", Password = "kt38hmapq" };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.ExchangeDeclare("shop_test", "direct", true);
                channel.QueueDeclare(queue: "shop_test_buyers",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.QueueBind("shop_test_buyers", "shop_test", "shop_test_buyers");

            }
            catch (BrokerUnreachableException ex)
            {
                _logger.LogError($"Byers backgroundService - " + ex.Message);
            }
            catch (RabbitMQClientException ex)
            {
                _logger.LogError($"Byers backgroundService - " + ex.Message);
            }
            
        }
    }
}
