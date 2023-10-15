using TelegramBotShit.Router;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramFinancialGameBot.Service.Router;

namespace TelegramBotShit.Bot;

public class BotRequestHandlers
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    private ChatsRouter _chatsRouter;

    public BotRequestHandlers(ChatsRouter chatsRouter)
    {
        _chatsRouter = chatsRouter;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var messageManager = BotMessageManager.Instance;
        MessageToSend messageToSend = MessageToSend.Empty;
        long chatId = 0;

        // todo make more clear and add more output parameters

        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    if (update.Message != null)
                    {
                        if (update.Message.Text == null || update.Message.Text == "")
                        {
                            messageToSend = MessageToSend.Empty;
                            break;
                        }

                        if (update.Message.Type != MessageType.Text)
                        {
                            Logger.Warn($"Unexpected value. MessageType is {update.Message.Type}, it has text: '{update.Message.Text}'");

                            messageToSend = MessageToSend.Empty;
                            break;
                        }

                        chatId = update.Message.Chat.Id;

                        messageManager.GetHistory(chatId).AddMessageId(update.Message.MessageId);

                        Console.WriteLine($"Принято входящее сообщение: chatId = {chatId}, UpdateType.Message");
                        Logger.Info($"Принято входящее сообщение: chatId = {chatId}, UpdateType.Message");

                        messageToSend =
                            await Task.Run(() => _chatsRouter.RouteMessage(chatId, update.Message), cancellationToken);
                    }
                    break;

                case UpdateType.CallbackQuery:
                    if (update.CallbackQuery != null)
                    {
                        if (update.CallbackQuery.Message == null)
                        {
                            throw new FormatException();
                        }

                        chatId = update.CallbackQuery.Message.Chat.Id;

                        Logger.Info($"Принято входящее сообщение: chatId = {chatId}, UpdateType.CallbackQuery");

                        messageToSend =
                            await Task.Run(() => _chatsRouter.RouteCallbackQuery(chatId, update.CallbackQuery), cancellationToken);
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Logger.Error("Unexpected exception in HandleUpdateAsync! " + ex);
        }

        if (messageToSend.Text != MessageToSend.Empty.Text)
        {
            var sender = messageManager.GetSender(chatId);
            var history = messageManager.GetHistory(chatId);
            
            sender.AddMessageToStack(messageToSend);
            
            var messages = sender.SendAllMessages().Result;

            history.AddMessageListIds(messages);
            
            Logger.Info($"Отправлено ответное сообщение: chatId = {chatId}");
        }
        else
        {
            if (update.Type == UpdateType.Message)
            {
                if (update.Message == null)
                {
                    Logger.Error("update.Message is null and UpdateType is Message. It can't be!");
                    throw new Exception();
                }
                
                if (!TransmittedDataRouter.Instance.HasUserTransmittedData(chatId))
                {
                    Logger.Debug("Обработано сообщение с ответом Message.Empty, полученное от пользователя в период отсутствия активности бота");
                    return;
                }

                await messageManager.GetHistory(chatId).DeleteLastMessage();
            }

            Logger.Info($"Обработан входящий запрос без ответного сообщения: chatId = {chatId}");
        }
        
        Logger.Debug($"Выполенна обработка входящего сообщения: chatId = {chatId} в методе HandleUpdateAsync");
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        string errorMessage = "empty";
        switch (exception)
        {
            case ApiRequestException:
            {
                var ex = exception as ApiRequestException;
                errorMessage = $"Telegram API Error:\n[{ex.ErrorCode}]\n{ex.Message}";
            }
                break;
            default:
            {
                errorMessage = exception.ToString();
            }
                break;
        }

        Logger.Error($"Ошибка поймана в методе HandlePollingErrorAsync, {errorMessage}");
        return Task.CompletedTask;
    }
}