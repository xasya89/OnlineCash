using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using OnlineCash.Models;
using System;

namespace OnlineCash.Services.DocSynchServices
{
    public class ArrivalSynchService
    {
        private readonly ILogger<ArrivalSynchService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public ArrivalSynchService(ILogger<ArrivalSynchService> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        public async Task SaveArrival(ArrivalSynchModel model)
        {
            using var scope = _scopeFactory.CreateScope();
            using var dbFactory = scope.ServiceProvider.GetService<shopContext>();

            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _configuration.GetSection("RabbitServer").Value,
                    UserName = _configuration.GetSection("RabbitUser").Value,
                    Password = _configuration.GetSection("RabbitPassword").Value
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare("shop_test", "direct", true);
                channel.QueueDeclare(queue: "shop_test_arrivals",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.QueueBind("shop_test_arrivals", "shop_test", "shop_test_arrivals");

            }
            catch (Exception ex)
            {
                _logger.LogError("Arrival background service error - " + ex.Message);
            }
        }
    }
}
