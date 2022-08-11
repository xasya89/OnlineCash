using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OnlineCash.Extensions
{
    public static class RabbitExtension
    {
        public static void Publish<T>(this IModel channel, string exchange, string routingkey, T model)
        {
            channel.BasicPublish(exchange: exchange,
                 routingKey: routingkey,
                 basicProperties: null,
                 body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model)));
        }

        public static void CreateStandartExchangeQueue(this IModel channel, string rabbitTemplate)
        {
            channel.ExchangeDeclare(rabbitTemplate, "direct", true);

            channel.QueueDeclare(queue: rabbitTemplate + "_goodbalance",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(rabbitTemplate + "_goodbalance", rabbitTemplate, rabbitTemplate + "_goodbalance");

            channel.QueueDeclare(queue: rabbitTemplate + "_notify",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(rabbitTemplate + "_notify", rabbitTemplate, rabbitTemplate + "_notify");
        }
    }
}
