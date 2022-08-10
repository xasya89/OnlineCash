using DatabaseBuyer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineCash.HostedServices
{
    public class BuyerBackgroundService : BackgroundService
    {
        private static List<Buyer> buyers = new();

        private readonly ILogger<BuyerBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceFactory;
        private readonly IConfiguration _configuration;

        public BuyerBackgroundService(IServiceScopeFactory serviceFactory,
            ILogger<BuyerBackgroundService> logger,
            IConfiguration configuration)
        {
            _serviceFactory = serviceFactory;
            _logger= logger;
            _configuration = configuration;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            /*
            try
            {
                using var scope = _serviceFactory.CreateScope();
                var _db = scope.ServiceProvider.GetService<shopbuyerContext>();
                var factory = new ConnectionFactory() { 
                    HostName = _configuration.GetSection("RabbitServer").Value, 
                    UserName = _configuration.GetSection("RabbitUser").Value, 
                    Password = _configuration.GetSection("RabbitPassword").Value
                };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("shop_test", "direct", true);
                    channel.QueueDeclare(queue: "shop_test_buyers",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    channel.QueueBind("shop_test_buyers", "shop_test", "shop_test_buyers");
                    while (!stoppingToken.IsCancellationRequested)
                    {

                        var buyersdb = await _db.Buyers.AsNoTracking().ToListAsync();
                        foreach (var b in buyersdb)
                        {
                            var buyer = buyers.Where(x => x.Uuid == b.Uuid).FirstOrDefault();
                            bool flagSend = false;
                            if (buyer == null)
                            {
                                buyers.Add(b);
                                flagSend = true;
                            }
                            if (buyer != null && (buyer.TemporyPercent != b.TemporyPercent || buyer.SpecialPercent != b.SpecialPercent || buyer.DiscountSum != b.DiscountSum))
                            {
                                buyer.TemporyPercent = b.TemporyPercent;
                                buyer.SpecialPercent = b.SpecialPercent;
                                buyer.DiscountSum = b.DiscountSum;
                                flagSend = true;
                            }
                            if (flagSend)
                                try
                                {
                                    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(b));
                                    channel.BasicPublish(exchange: "shop_test",
                                 routingKey: "shop_test_buyers",
                                 basicProperties: null,
                                 body: body);
                                }
                                catch (RabbitMQClientException) { };
                        }
                        await Task.Delay(TimeSpan.FromSeconds(20));
                    }
                }
            }
            catch(BrokerUnreachableException ex)
            {
                _logger.LogError($"Byers backgroundService - " + ex.Message);
            }
            catch(RabbitMQClientException ex)
            {
                _logger.LogError($"Byers backgroundService - "+ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Byers backgroundService - " + ex.Message);
            }
            */
        }
    }
}
