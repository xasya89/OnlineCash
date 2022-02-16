using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationServer.EventsBus
{
    public static class TelegramNotifyEventBus
    {
        public delegate Task SendHandler(string Owner, string Message, string Url);
        public static event SendHandler Send;
        public static void SendMessage(string Owner, string Message, string Url)
        {
            Send?.Invoke(Owner, Message, Url);
        }
    }
}
