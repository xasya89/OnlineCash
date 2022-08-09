using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using OnlineCashRmk.DataModels;
using OnlineCashRmk.Models;
using Microsoft.Extensions.Configuration;

namespace OnlineCashRmk.Services
{
    public class RabbitBackgroundService : BackgroundService
    {
        private readonly ILogger<RabbitBackgroundService> _logger;
        private IServiceScopeFactory _scopeService;
        private readonly IConfiguration _configuration;
        private object _lock = new object();
        public RabbitBackgroundService(
            IServiceScopeFactory scopeFactory, 
            IConfiguration configuration,
            ILogger<RabbitBackgroundService> logger)
        {
            _scopeService = scopeFactory;
            _configuration = configuration;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeService.CreateScope();
            var dbFactory = scope.ServiceProvider.GetService<IDbContextFactory<DataContext>>();
            using var db = dbFactory.CreateDbContext();
            try
            {
                var factory = new ConnectionFactory() { 
                    HostName = _configuration.GetSection("RabbitServer").Value, 
                    UserName = _configuration.GetSection("RabbitUser").Value, 
                    Password = _configuration.GetSection("RabbitPassword").Value
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare("shop_test", "direct", true);
                channel.QueueDeclare(queue: "shop_test_buyers",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.QueueBind("shop_test_buyers", "shop_test", "shop_test_buyers");

                var telegramConsumer = new EventingBasicConsumer(channel);
                telegramConsumer.Received += async (ch, ex) =>
                {
                    var body = ex.Body.ToArray();
                    var str = Encoding.UTF8.GetString(body, 0, body.Length);
                    var buyer = JsonSerializer.Deserialize<Buyer>(str);
                    lock (_lock)
                    {
                        var buyerDb = db.Buyers.Where(b => b.Uuid == buyer.Uuid).FirstOrDefault();
                        if (buyerDb == null)
                        {
                            buyer.Id = 0;
                            db.Buyers.Add(buyer);
                        }
                        else
                        {
                            buyerDb.SpecialPercent = buyer.SpecialPercent;
                            buyerDb.TemporyPercent = buyer.TemporyPercent;
                            buyerDb.DiscountSum = buyer.DiscountSum;
                        };
                        db.SaveChanges();
                    }
                };
                channel.BasicConsume("shop_test_buyers", true, telegramConsumer);
                while (!stoppingToken.IsCancellationRequested)
                    await Task.Delay(TimeSpan.FromSeconds(3));
            }
            catch (RabbitMQClientException ex)
            {
                _logger.LogError("Rabbit error - " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Rabbit error - " + ex.Message);
            };
        }
    }
}
