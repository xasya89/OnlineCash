using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationServer.DatabaseModels
{
    public class NotifySubscribe
    {
        public int Id { get; set; }
        public int NotificationClientId { get; set; }
        public NotificationClient NotificationClient { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
