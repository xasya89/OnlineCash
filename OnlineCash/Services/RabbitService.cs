using DnsClient.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineCash.Extensions;
using RabbitMQ.Client;

namespace OnlineCash.Services
{
    public class RabbitService
    {
        public struct QueueNames
        {
            public static string GoodBalance = "goodbalance";
            public static string Notify = "notify";
        }

        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitService> _logger;
        private string rabbitTemplate = "";
        public RabbitService(IConfiguration configuration, ILogger<RabbitService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            rabbitTemplate = _configuration.GetSection("RabbitTemplate").Value;
        }

        public void Send<T>(string queuename, T model)
        {

            var factory = new ConnectionFactory()
            {
                HostName = _configuration.GetSection("RabbitServer").Value,
                UserName = _configuration.GetSection("RabbitUser").Value,
                Password = _configuration.GetSection("RabbitPassword").Value
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(rabbitTemplate, "direct", true);
            channel.QueueDeclare(queue: rabbitTemplate + "_"+queuename,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(rabbitTemplate + "_"+queuename, rabbitTemplate, rabbitTemplate + "_"+queuename);

            channel.Publish<T>(rabbitTemplate, rabbitTemplate + "_"+queuename, model);
        }
    }
}
