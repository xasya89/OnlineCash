using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using RabbitMqMessages;
using BuyersServiceApp.Models;

namespace BuyersServiceApp.Consumers
{
    class BuyerSellConsumer : IConsumer<BuyerSellMessage>
    {
        shopbuyersContext _db;
        public BuyerSellConsumer(shopbuyersContext db)
        {
            _db = db;
        }
        public async Task Consume(ConsumeContext<BuyerSellMessage> context)
        {
            _db.Buyers.Add(new Buyer { Uuid = context.Message.Uuid, DiscountSum = context.Message.Sum, Phone = context.Message.Phone });
            await _db.SaveChangesAsync();
            Console.WriteLine("recive");
        }
    }
}
