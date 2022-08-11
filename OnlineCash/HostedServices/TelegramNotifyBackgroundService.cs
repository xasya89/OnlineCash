using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using OnlineCash.DataBaseModels;
using OnlineCash.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using OnlineCash.Extensions;

namespace OnlineCash.HostedServices
{
    public class TelegramNotifyBackgroundService : BackgroundService
    {
        private readonly ILogger<TelegramNotifyBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private object _lock = new object();
        private string rabbitTemplate;
        private string rabbitId;
        private string rabbitServer;
        private string rabbitUser;
        private string rabbitPassword;
        private string serverName;
        private ITelegramBotClient _botClient;

        public TelegramNotifyBackgroundService(ILogger<TelegramNotifyBackgroundService> logger, 
            IConfiguration configuration, 
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            rabbitTemplate = configuration.GetSection("RabbitTemplate").Value;
            rabbitId = configuration.GetConnectionString("Telegram");
            rabbitServer = configuration.GetSection("RabbitServer").Value;
            rabbitUser = configuration.GetSection("RabbitUser").Value;
            rabbitPassword = configuration.GetSection("RabbitPassword").Value;
            serverName = configuration.GetSection("ServerName").Value;
            _scopeFactory = scopeFactory;
            _botClient = new TelegramBotClient(configuration.GetConnectionString("Telegram"));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            List<TelegramUser> telegramUsers;
            using (var scope = _scopeFactory.CreateScope())
            using (var db = scope.ServiceProvider.GetService<shopContext>())
                telegramUsers = await db.TelegramUsers.AsNoTracking().ToListAsync();
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = rabbitServer,
                    UserName = rabbitUser,
                    Password = rabbitPassword
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.CreateStandartExchangeQueue(rabbitTemplate);

                var arrivalConsumer = new EventingBasicConsumer(channel);
                arrivalConsumer.Received += async (ch, ex) =>
                {
                    var body = ex.Body.ToArray();
                    var str = Encoding.UTF8.GetString(body, 0, body.Length);
                    var model = JsonSerializer.Deserialize<TelegramNotifyModel>(str);

                    await Send(telegramUsers, model.Message, model.Url);
                };
                channel.BasicConsume(rabbitTemplate+"_notify", true, arrivalConsumer);
                while (!stoppingToken.IsCancellationRequested)
                    await Task.Delay(TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                _logger.LogError("Telegram notify background service error - " + ex.Message);
            }
        }

        private async Task Send(List<TelegramUser> users, string message, string url = null)
        {
            if(users!=null)
            foreach (var tgUser in users)
            {
                if (url != null)
                {
                    var keyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("Открыть", serverName + "/" + url));
                    await _botClient.SendTextMessageAsync(tgUser.ChatId, message, replyMarkup: keyboard);
                }
                else
                    await _botClient.SendTextMessageAsync(tgUser.ChatId, message);
            }
        }
    }
}
