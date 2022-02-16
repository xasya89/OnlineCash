using NotificationServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Extensions.Polling;
using NotificationServer.EventsBus;

namespace NotificationServer.Service
{
    public class TelegramNotificationService : IUserNotificationService
    {
        public TelegramNotificationService(IConfiguration configuration)
        {
        }

        public void Send(NotificationMessage message)
        {
            TelegramNotifyEventBus.SendMessage(message.Owner, message.Message, message.Url);
        }

        public Task SendAsync(NotificationMessage messaage)
        {
            throw new NotImplementedException();
        }
    }
}
