using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;

namespace OnlineCash.HostedServices
{
    public class TelegramHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private TelegramBotClient botClient;
        private List<NewUser> RegisterUsers = new List<NewUser>();
        public TelegramHostedService(IServiceScopeFactory serviceScope) => _scopeFactory = serviceScope;
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
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
        private async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}");
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
        private async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {

        }

        private Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            return Task.CompletedTask;
        }

        private Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            return Task.CompletedTask;
        }

        private async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {

            using var scope = _scopeFactory.CreateScope();
            var _db = scope.ServiceProvider.GetRequiredService<shopContext>();
            if (message.Type != MessageType.Text)
                return;

            var registerUser = RegisterUsers.Where(u => u.ChatId == message.Chat.Id).FirstOrDefault();
            if (registerUser == null)
            {
                registerUser = new NewUser
                {
                    ChatId = message.Chat.Id,
                    RegisterStatus = UserRegisterStatus.New
                };
                RegisterUsers.Add(registerUser);
            };
            if (registerUser.RegisterStatus == UserRegisterStatus.New)
            {
                await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                                text: "Введите имя пользователя:");
                registerUser.RegisterStatus = UserRegisterStatus.InputLogin;
                return;
            }
            if (registerUser.RegisterStatus == UserRegisterStatus.InputLogin)
            {
                registerUser.Login = message.Text;
                await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                                text: "Введите пароль:");
                registerUser.RegisterStatus = UserRegisterStatus.InputPassword;
                return;
            }
            if (registerUser.RegisterStatus == UserRegisterStatus.InputPassword)
            {
                registerUser.Password = message.Text;
                var dbUser = await _db.Users.Where(u => u.Email ==registerUser.Login & u.Password==registerUser.PasswordMd5).FirstOrDefaultAsync();
                if (dbUser == null)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                                    text: "Неверный логи или пароль. Нажмите /start для повторной авторизации");
                    registerUser.RegisterStatus = UserRegisterStatus.New;
                    return;
                }
            }

            await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Авторизация пройдена, тпереь Вам будут приходить уведомления");
            registerUser.RegisterStatus = UserRegisterStatus.Complited;
            _db.TelegramUsers.Add(new DataBaseModels.TelegramUser { ChatId = message.Chat.Id });
            await _db.SaveChangesAsync();
            return;
        }

        private enum UserRegisterStatus
        {
            New,
            InputLogin,
            InputPassword,
            Complited
        }
        private class NewUser
        {
            public long ChatId { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public string PasswordMd5 { get => Password.CreateMD5(); }
            public UserRegisterStatus RegisterStatus { get; set; } = UserRegisterStatus.InputLogin;
        }
    }
}
