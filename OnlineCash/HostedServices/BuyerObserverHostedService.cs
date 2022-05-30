﻿using DatabaseBuyer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineCash.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hangfire;

namespace OnlineCash.HostedServices
{
    public class BuyerObserverHostedService : IDisposable
    {
        private static List<Buyer> buyers = new();

        private readonly shopbuyerContext _db;
        private readonly IHubContext<DiscountAndBuyerHub> _hub;
        public BuyerObserverHostedService(shopbuyerContext db, IHubContext<DiscountAndBuyerHub> hub)
        {
            _db = db;
            _hub = hub;
        }

        public void Dispose()
        {

        }

        public async Task Run(IJobCancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await StartAsync();
        }

        public async Task StartAsync()
        {
            var buyersdb = await _db.Buyers.ToListAsync();
            foreach (var b in buyersdb)
            {
                var buyer = buyers.Where(x => x.Uuid == b.Uuid).FirstOrDefault();
                if (buyer == null)
                {
                    buyers.Add(b);
                    await _hub.Clients.All.SendAsync("buyer", b);
                }
                if(buyer!=null && (buyer.TemporyPercent!=b.TemporyPercent || buyer.SpecialPercent!=b.SpecialPercent || buyer.DiscountSum!=b.DiscountSum))
                {
                    buyer.TemporyPercent = b.TemporyPercent;
                    buyer.SpecialPercent = b.SpecialPercent;
                    buyer.DiscountSum = b.DiscountSum;
                    await _hub.Clients.All.SendAsync("buyer", b);
                }
            }

        }
    }
}
