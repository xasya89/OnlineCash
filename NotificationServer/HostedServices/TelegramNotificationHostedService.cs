using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore;
using NotificationServer.DatabaseModels;
using NotificationServer.EventsBus;

namespace NotificationServer.HostedServices
{
    public class TelegramNotificationHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private TelegramBotClient botClient;

        public TelegramNotificationHostedService(IServiceScopeFactory scopeFactory) =>
            _scopeFactory = scopeFactory;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            botClient = new TelegramBotClient(configuration.GetConnectionString("Telegram"));
            using var cts = new CancellationTokenSource();

            ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
            botClient.StartReceiving(HandleUpdateAsync,
                               HandleErrorAsync,
                               receiverOptions,
                               cts.Token);

            TelegramNotifyEventBus.Send += SendNotifyByShop;
            await botClient.SendTextMessageAsync(chatId: 305677373,
                                                                    text: "<a href=\"https://shop7.exp-tech.com/Arrivals/Edit?ArrivalId=76\">dfdf</a>",
                                                                    parseMode: ParseMode.Html
                                                                    );
        }

        async Task SendNotifyByShop(string owner, string message, string url)
        {
            using var scope = _scopeFactory.CreateScope();
            var _db = scope.ServiceProvider.GetRequiredService<shop_registrationContext>();
            var shop = await _db.Shops.Where(s => s.ServerUrl == owner).FirstOrDefaultAsync();
            var organization = await (from o in _db.Organizations
                               join s in _db.Shops on o.Id equals s.OrganizationId
                               where s.ServerUrl == owner
                               select o).FirstOrDefaultAsync();
            if (organization == null)
                return;
            var clients = await (from c in _db.NotificationClients
                                 join s in _db.NotifySubscribes on c.Id equals s.NotificationClientId
                                 where s.OrganizationId == organization.Id
                                 select c).ToListAsync();
            foreach(var client in clients)
            {
                string html = $"<b>{shop?.Name}</b>\n{message}\n<a href='{url}'>посмотреть</a>";
                try
                {
                    await botClient.SendTextMessageAsync(chatId: client.ChatId,
                                                                    text: html,
                                                                    parseMode: ParseMode.Html
                                                                    );
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message!),
                UpdateType.EditedMessage => BotOnMessageReceived(botClient, update.EditedMessage!),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery!),
                UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery!),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult!),
                _ => UnknownUpdateHandlerAsync(botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        private enum UserRegisterStatus
        {
            New,
            InputInn,
            InputPassword,
            Complited
        }

        private class NewUser
        {
            public long ChatId { get; set; }
            public UserRegisterStatus RegisterStatus { get; set; } = UserRegisterStatus.InputInn;
            public string Inn { get; set; }
        }
        private List<NewUser> RegisterUsers = new List<NewUser>();

        private async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {

            using var scope = _scopeFactory.CreateScope();
            var _db = scope.ServiceProvider.GetRequiredService<shop_registrationContext>();
            if (message.Type != MessageType.Text)
                return;

            var registerUser = RegisterUsers.Where(u => u.ChatId == message.Chat.Id).FirstOrDefault();
            if ( registerUser==null)
            {
                registerUser = new NewUser
                {
                    ChatId = message.Chat.Id,
                    RegisterStatus = UserRegisterStatus.New
                };
                RegisterUsers.Add(registerUser);
            };
            if(registerUser.RegisterStatus==UserRegisterStatus.New)
            {
                await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                                text: "Для получения уведомлений введите ИНН организации:");
                registerUser.RegisterStatus = UserRegisterStatus.InputInn;
                return;
            }
            if (registerUser.RegisterStatus == UserRegisterStatus.InputInn)
            {
                var organization = await _db.Organizations.Where(o => o.Inn == message.Text).FirstOrDefaultAsync();
                if (organization == null)
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                                text: "ИНН организации не найден в списке подключенных для уведомления. Повторите ввод ИНН:");
                else
                {
                    registerUser.Inn = message.Text;
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                                text: $"Для подключение к {organization.OrgName} введите пароль:");
                    registerUser.RegisterStatus = UserRegisterStatus.InputPassword;
                }
                return;
            }
            if(registerUser.RegisterStatus==UserRegisterStatus.InputPassword)
            {
                var organization = await _db.Organizations.Where(o => o.Inn == registerUser.Inn & o.Secret == message.Text).FirstOrDefaultAsync();
                if(organization==null)
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                                text: "Пароль введен не верно. Попробуйте еше раз:");
                else
                {
                    var notifiClient = await _db.NotificationClients.Where(c => c.ChatId == message.Chat.Id).FirstOrDefaultAsync();
                    if(notifiClient==null)
                    {
                        notifiClient = new NotificationClient { ChatId = message.Chat.Id };
                        _db.NotificationClients.Add(notifiClient);
                    }
                    var subscribe = await _db.NotifySubscribes.Where(s => s.NotificationClientId == notifiClient.Id & s.OrganizationId == organization.Id).FirstOrDefaultAsync();
                    if (subscribe == null)
                        _db.NotifySubscribes.Add(new NotifySubscribe { NotificationClient = notifiClient, Organization = organization });
                    await _db.SaveChangesAsync();
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                                text: $"Подписка успешно оформлена.\n /start - для получения уведомлений от другой организации");
                    registerUser.RegisterStatus = UserRegisterStatus.New;
                }
                return;
            };
        }

        // Process Inline Keyboard callback data
        private async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}");
        }

        private async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {

            InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "3",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent(
                    "hello"
                )
            )
        };

            await botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                                   results: results,
                                                   isPersonal: true,
                                                   cacheTime: 0);
        }

        private Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            return Task.CompletedTask;
        }

        private Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            return Task.CompletedTask;
        }
    }
}
