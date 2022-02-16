using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationServer.DatabaseModels
{
    public class Shop
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string ServerUrl { get; set; }

    }
}
