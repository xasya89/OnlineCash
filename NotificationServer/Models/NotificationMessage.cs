using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationServer.Models
{
    public class NotificationMessage
    {
        public string Owner { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}
