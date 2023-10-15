using NLog;
using Telegram.Bot.Types;

namespace TelegramBotShit.Bot;

public class BotNotificationSender
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private static BotNotificationSender _notificationSender;

    private BotResponder _botResponder;
    
    private BotNotificationSender(BotResponder botResponder)
    {
        _botResponder = botResponder;
    }

    public static BotNotificationSender Instance
    {
        get
        {
            if (_notificationSender == null)
            {
                Logger.Error("BotNotificationSender not initialized");
                throw new Exception();
            }

            return _notificationSender;
        }
    }

    public static bool Create(BotResponder botResponder)
    {
        if (_notificationSender == null)
        {
            _notificationSender = new BotNotificationSender(botResponder);
            Logger.Debug("BotNotificationSender is initialized");
            return true;
        }

        return false;
    }
    
    public async Task<Message> SendNotificationMessage(MessageToSend messageToSend, long chatId)
    {
        Task<Message> task = SendMessage(messageToSend, chatId);
        
        Message message = task.Result;
        
        BotMessageManager.Instance.GetHistory(chatId).AddMessageId(message.MessageId);
        
        var sleepValue = BotStatisticManager.GetInstance().SleepValue;
        await Task.Run(() => Thread.Sleep(sleepValue));
        
        return task.Result;
    }
    
    public async Task<Message> SendAnchoredNotificationMessage(MessageToSend messageToSend, long chatId)
    {
        Task<Message> task = SendMessage(messageToSend, chatId);
        
        Message message = task.Result;
        
        BotMessageManager.Instance.GetHistory(chatId).AddAnchoredMessagesId(message.MessageId);

        var sleepValue = BotStatisticManager.GetInstance().SleepValue;
        await Task.Run(() => Thread.Sleep(sleepValue));
        
        return task.Result;
    }
    
    private Task<Message> SendMessage(MessageToSend message, long chatId)
    {
        BotStatisticManager.GetInstance().AddWorkLoad();
        
        return _botResponder.SendText(message, chatId);
    }
}