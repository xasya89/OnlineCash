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
    public class ArrivalReciveBackgroundService:BackgroundService
    {
        private readonly ILogger<ArrivalReciveBackgroundService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private object _lock = new object();

        public ArrivalReciveBackgroundService(ILogger<ArrivalReciveBackgroundService> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
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
                channel.QueueDeclare(queue: "shop_test_arrivals",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.QueueBind("shop_test_arrivals", "shop_test", "shop_test_arrivals");
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
                        var model = JsonSerializer.Deserialize<ArrivalSynchModel>(str);

                        if (db.Suppliers.Where(s => s.Id == model.SupplierId).FirstOrDefault() == null)
                            throw new Exception($"Поставщик supplierId - {model.SupplierId} не найден");
                        var goods = db.Goods.Include(g => g.GoodPrices.Where(gp => gp.ShopId == 1)).AsNoTracking().ToList();
                        foreach (var mGood in model.ArrivalGoods)
                            if (goods.Where(g => g.Uuid == mGood.GoodUuid).FirstOrDefault() == null)
                                throw new Exception($"Товар uuid {mGood.GoodUuid} не найден");

                        bool autoSuccess = _configuration.GetSection("AutoSuccessFromCash").Value == "1";
                        var arrival = new Arrival
                        {
                            Num = model.Num,
                            DateArrival = model.DateArrival,
                            ShopId = 1,
                            SupplierId = model.SupplierId,
                            SumSell = 0,
                            isSuccess = autoSuccess
                        };
                        db.Arrivals.Add(arrival);
                        List<ArrivalGood> arrivalGoods = new List<ArrivalGood>();
                        foreach (var mGood in model.ArrivalGoods)
                        {
                            var good = goods.Where(g => g.Uuid == mGood.GoodUuid).FirstOrDefault();
                            arrivalGoods.Add(new ArrivalGood
                            {
                                Arrival = arrival,
                                GoodId = good.Id,
                                Price = mGood.Price,
                                PriceSell = good.GoodPrices.FirstOrDefault().Price,
                                Count = mGood.Count,
                                Nds = mGood.Nds,
                                ExpiresDate = mGood.ExpiresDate
                            });
                        }
                        db.ArrivalGoods.AddRange(arrivalGoods);
                        arrival.SumNds = arrival.ArrivalGoods.Sum(a => a.SumNds);
                        arrival.SumArrival = arrival.ArrivalGoods.Sum(a => a.Sum);
                        arrival.SumSell = arrival.ArrivalGoods.Sum(a => a.SumSell);
                        db.SaveChanges();

                        List<GoodBalanceSynchModel> balances = new();
                        foreach (var arrivalGood in arrivalGoods)
                            balances.Add(new GoodBalanceSynchModel
                            {
                                DocumentId = arrival.Id,
                                DocumentDate=arrival.DateArrival,
                                TypeDoc = TypeDocs.Arrival,
                                GoodId = arrivalGood.GoodId,
                                Count = arrivalGood.Count
                            });
                        var bodyBalances = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(balances));
                        channel.BasicPublish(exchange: "shop_test",
                         routingKey: "shop_test_goodbalance",
                         basicProperties: null,
                         body: bodyBalances);
                    };

                    //channel.BasicAck(ex.DeliveryTag, true);
                };
                channel.BasicConsume("shop_test_arrivals", true, arrivalConsumer);
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
