using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationServer.DatabaseModels
{
    public class Organization
    {
        public int Id { get; set; }
        public string OrgName { get; set; }
        public string Inn { get; set; }
        public string Secret { get; set; }
        public List<Shop> Shops { get; set; }
        public List<NotifySubscribe> NotifySubscribes { get; set; }
    }
}
