using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotificationServer.Service
{
    public class Handlers
    {
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
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
        private static List<NewUser> RegisterUsers = new List<NewUser>();

        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Type != MessageType.Text)
                return;
            var registerUser = RegisterUsers.Where(u => u.ChatId == message.Chat.Id).FirstOrDefault();
            if(registerUser==null)

            if (registerUser!=null)
            {
                if(registerUser.RegisterStatus==UserRegisterStatus.New && message.Text=="312301001")
                {
                    registerUser.RegisterStatus = UserRegisterStatus.InputPassword;
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Введите пароль:");
                    return;
                }
                if (registerUser.RegisterStatus == UserRegisterStatus.New && message.Text != "312301001")
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "ИНН не найден в базе, введите корректный ИНН:");

                if (registerUser.RegisterStatus == UserRegisterStatus.InputPassword && message.Text == "kt38hmapq")
                {
                    registerUser.RegisterStatus = UserRegisterStatus.Complited;
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Регистрация выполнена! Теперь вы будете получать уведомления");
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "/subscribe - если вы хотите еще добавить организацию");
                    return;
                }
                if (registerUser.RegisterStatus == UserRegisterStatus.InputPassword && message.Text != "kt38hmapq")
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Не верный парольповторите ввод:");
                return;
            }

            var action = message.Text!.Split(' ')[0] switch
            {
                "/subscribe" => SendSubscribe(botClient, message),
                _ => Usage(botClient, message)
            };
            Message sentMessage = await action;

            static async Task<Message> SendSubscribe(ITelegramBotClient botClient, Message message)
            {
                RegisterUsers.Add(new NewUser { ChatId = message.Chat.Id, RegisterStatus = UserRegisterStatus.New });
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Введите ИНН организации:");
            }

            static async Task<Message> Usage(ITelegramBotClient botClient, Message message)
            {
                const string usage = "Выберите действие:\n" +
                                     "/subscribe  - начать получать уведомления о движении в магахине\n";

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: usage,
                                                            replyMarkup: new ReplyKeyboardRemove());
            }
        }

        // Process Inline Keyboard callback data
        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}");
        }

        private static async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
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

        private static Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            return Task.CompletedTask;
        }

        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            return Task.CompletedTask;
        }
    }
}
