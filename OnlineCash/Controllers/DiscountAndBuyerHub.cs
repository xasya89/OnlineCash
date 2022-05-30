using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using DatabaseBuyer;

namespace OnlineCash.Controllers
{
    public class DiscountAndBuyerHub:Hub
    {
        public const string HabUrl = "/discount_and_buyer_hub";
        public async Task SendBuyer(Buyer buyer) =>
            await Clients.All.SendAsync("buyer", buyer);
    }
}
