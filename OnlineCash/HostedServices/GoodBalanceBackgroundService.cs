using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;
using System.Text.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineCash.Models;
using OnlineCash.DataBaseModels;
using System.Collections.Generic;

namespace OnlineCash.HostedServices
{
    public class GoodBalanceBackgroundService : BackgroundService
    {
        private readonly ILogger<ArrivalReciveBackgroundService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private object _lock = new object();

        public GoodBalanceBackgroundService(ILogger<ArrivalReciveBackgroundService> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            using var db = scope.ServiceProvider.GetService<shopContext>();
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
                channel.QueueDeclare(queue: "shop_test_goodbalance",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.QueueBind("shop_test_goodbalance", "shop_test", "shop_test_goodbalance");

                var arrivalConsumer = new EventingBasicConsumer(channel);
                arrivalConsumer.Received += (ch, ex) =>
                {
                    lock (_lock)
                    {
                        var body = ex.Body.ToArray();
                        var str = Encoding.UTF8.GetString(body, 0, body.Length);
                        var model = JsonSerializer.Deserialize<List<GoodBalanceSynchModel>>(str);
                        Console.WriteLine(model);

                    };

                    //channel.BasicAck(ex.DeliveryTag, true);
                };
                channel.BasicConsume("shop_test_goodbalance", true, arrivalConsumer);
                while (!stoppingToken.IsCancellationRequested)
                    await Task.Delay(TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                _logger.LogError("Arrival background service error - " + ex.Message);
            }
        }
    }
}
