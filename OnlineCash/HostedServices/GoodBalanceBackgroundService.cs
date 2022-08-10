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
using Dapper;
using MySql.Data.MySqlClient;

namespace OnlineCash.HostedServices
{
    public class GoodBalanceBackgroundService : BackgroundService
    {
        private readonly ILogger<GoodBalanceBackgroundService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private object _lock = new object();

        public GoodBalanceBackgroundService(ILogger<GoodBalanceBackgroundService> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
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

                        using var con =new MySqlConnection(_configuration.GetConnectionString("MySQL"));
                        
                        DateTime? minPeriod = con.ExecuteScalar<DateTime?>("SELECT MIN(period) FROM goodcountbalances");
                        DateTime? maxPeriod = con.ExecuteScalar<DateTime?>("SELECT MAX(period) FROM goodcountbalances");
                        if(minPeriod==null)
                        {
                            con.Execute($@"INSERT INTO goodcountbalances (Period, GoodId, Count) 
SELECT STR_TO_DATE('{DateTime.Now.ToString("01.MM.yyyy")}','%d.%m.%Y'), id, 0 FROM Goods");
                            con.Execute($@"INSERT INTO goodcountbalancecurrents (GoodId, Count) 
SELECT id, 0 FROM Goods");
                        }
                        var periodCur = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        if(DateTime.Compare((DateTime)maxPeriod,periodCur)<0)
                            for(DateTime p=maxPeriod.Value.AddMonths(1); DateTime.Compare(p, periodCur)<=0; p=p.AddMonths(1))
                                con.Execute($@"INSERT INTO goodcountbalances (Period, GoodId, Count) 
SELECT STR_TO_DATE('{p.ToString("01.MM.yyyy")}','%d.%m.%Y'), g.id, IFNULL(c.Count, 0)
FROM Goods g LEFT JOIN goodcountbalancecurrents c ON g.id=c.GoodId");
                        maxPeriod = periodCur;
                        foreach(GoodBalanceSynchModel good in model)
                        {
                            bool flagSearch = con.ExecuteScalar<bool>($"SELECT IF(COUNT(*)>0,TRUE, FALSE) FROM goodcountbalancecurrents WHERE goodid={good.GoodId}");
                            if (!flagSearch)
                            {
                                con.Execute($"INSERT INTO goodcountbalancecurrents (GoodId, Count) VALUES ({good.GoodId}, 0)");
                                for(DateTime p=minPeriod.Value; p<=maxPeriod.Value;p=p.AddMonths(1))
                                    con.Execute($"INSERT INTO goodcountbalances (Period, GoodId, Count) VALUES (STR_TO_DATE('{p.ToString("01.MM.yyyy")}','%d.%m.%Y'), {good.GoodId}, 0)");
                            }
                            else
                            {
                                con.Execute(
                                    "UPDATE goodcountbalancecurrents SET Count=Count + @Count WHERE goodId=@GoodId",
                                    new {Count=good.Count, GoodId=good.GoodId}
                                    );
                                con.Execute(
                                    "UPDATE goodcountbalances SET Count=Count + @Count WHERE Period=@Period AND goodId=@GoodId",
                                    new { Period=maxPeriod, Count = good.Count, GoodId = good.GoodId }
                                    );
                                con.Execute(
                                    "INSERT INTO goodcountdochistories (Date, GoodId, DocId, TypeDoc, Count) VALUES (@Date, @GoodId, @DocId, @TypeDoc, @Count)",
                                    new { DAte=good.DocumentDate, GoodId=good.GoodId, DocId=good.DocumentId, TypeDoc=good.TypeDoc, Count=good.Count }
                                    );
                            }
                        }
                        Console.WriteLine(minPeriod);
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
