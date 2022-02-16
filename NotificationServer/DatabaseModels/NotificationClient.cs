using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationServer.DatabaseModels
{
    public class NotificationClient
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public List<NotifySubscribe> NotifySubscribes { get; set; }
    }
}
