using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using BuyersServiceApp.Models;
using System.Threading.Tasks;
using RabbitMqMessages;

namespace BuyersServiceApp
{
    class Program
    {
        static IConfiguration _configuration;
        static async Task Main(string[] args)
        {
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: true).Build();
            var services = new ServiceCollection();
            GetServices(services);
            ServiceProvider provider = services.BuildServiceProvider();
            {
                var db= provider.GetRequiredService<shopbuyersContext>();
                db.Database.Migrate();
                var publisher = provider.GetRequiredService<IPublishEndpoint>();
                await publisher.Publish<BuyerSellMessage>(new { Uuid=Guid.NewGuid(), Phone="", Sum =0 });
                var bus = provider.GetRequiredService<IBusControl>();
                await bus.StartAsync();
                /*
                Task.Run(async () =>
                {
                    await bus.StopAsync();
                }).Start();
                */
            }
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }

        public static void GetServices(IServiceCollection services) =>
            services.AddDbContext<shopbuyersContext>(opt =>
            {
                opt.UseMySql(_configuration.GetConnectionString("MySQL"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.30-mysql"));
            })
            .AddMassTransit(x =>
            {
                x.AddConsumer<Consumers.BuyerSellConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_configuration.GetSection("RabbitMqHost").Value, "/", h =>
                    {
                        h.Username(_configuration.GetSection("RabbitMqUser").Value);
                        h.Password(_configuration.GetSection("RabbitMqPass").Value);
                    });
                    cfg.ReceiveEndpoint("buyersell-listener", e =>
                    {
                        e.ConfigureConsumer<Consumers.BuyerSellConsumer>(context);
                    });
                });
            });

    }
}
