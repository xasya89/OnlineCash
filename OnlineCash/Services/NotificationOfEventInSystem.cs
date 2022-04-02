using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using EasyNetQ;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace OnlineCash.Services
{
    public class NotificationOfEventInSystemService
    {
        private IConfiguration _configuration;
        private IBus _bus;
        private readonly string _serverNotification;
        private readonly shopContext _db;
        private ITelegramBotClient _botClient;
        public NotificationOfEventInSystemService(IConfiguration configuration, shopContext db)
        {
            _configuration = configuration;
            _serverNotification = _configuration.GetConnectionString("ServerNotification");
            _db = db;
            _botClient = new TelegramBotClient(configuration.GetConnectionString("Telegram"));
        }
        public async Task Send(string message, string url = null)
        {
            //await _bus.PubSub.PublishAsync<NotificationMessage>(new NotificationMessage { Notification = message, Url = url }, "NotificationOfEventInSystem");
            /*
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
            */
            foreach (var tgUser in await _db.TelegramUsers.ToListAsync())
            {
                url = url != null ? "http://" + _configuration.GetSection("ServerName").Value + "/" + url : null;
                string html = $"{message}\n<a href='{url}'>посмотреть</a>";
                await _botClient.SendTextMessageAsync(chatId: tgUser.ChatId,
                                                                    text: html,
                                                                    parseMode: ParseMode.Html
                                                                    );
            }
        }
    }
    class NotificationMessage
    {
        public string Owner { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}
