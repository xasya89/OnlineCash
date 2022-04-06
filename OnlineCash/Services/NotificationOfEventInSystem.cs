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
using Telegram.Bot.Types.ReplyMarkups;

namespace OnlineCash.Services
{
    public class NotificationOfEventInSystemService
    {
        private IConfiguration _configuration;
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
            foreach (var tgUser in await _db.TelegramUsers.ToListAsync())
            {
                if(url!=null)
                {
                    var keyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("Открыть", _configuration.GetSection("ServerName").Value + "/" + url));
                    await _botClient.SendTextMessageAsync(tgUser.ChatId, message, replyMarkup: keyboard);
                }
                else
                    await _botClient.SendTextMessageAsync(tgUser.ChatId, message);
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
