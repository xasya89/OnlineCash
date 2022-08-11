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
    }
}
