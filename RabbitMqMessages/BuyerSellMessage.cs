using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqMessages
{
    public interface BuyerSellMessage
    {
        public Guid Uuid { get; set; }
        public string Phone { get; set; }
        public decimal Sum { get; set; }
    }
}
