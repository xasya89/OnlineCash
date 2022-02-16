using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using EasyNetQ;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;

namespace OnlineCash.Services
{
    public class NotificationOfEventInSystemService
    {
        private IConfiguration _configuration;
        private IBus _bus;
        private readonly string _serverNotification;
        public NotificationOfEventInSystemService(IConfiguration configuration)
        {
            _configuration = configuration;
            _serverNotification = _configuration.GetConnectionString("ServerNotification");

            //_bus = RabbitHutch.CreateBus(configuration.GetConnectionString("Rabbit"));
        }
        public async Task Send(string message, string url = null)
        {
            //await _bus.PubSub.PublishAsync<NotificationMessage>(new NotificationMessage { Notification = message, Url = url }, "NotificationOfEventInSystem");
            try
            {
                
                await
            $"{_serverNotification}/api/notifications".PostJsonAsync(
                new NotificationMessage {
                    Owner = _configuration.GetSection("ServerName").Value,
                    Message = message,
                    Url = url != null ? "http://"+ _configuration.GetSection("ServerName").Value + "/" + url : null}
                );
            }
            catch (FlurlHttpException ex) { }
            catch (Exception) { };
        }
    }
    class NotificationMessage
    {
        public string Owner { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}
