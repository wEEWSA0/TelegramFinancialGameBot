using TelegramBotShit.Router;
using TelegramBotShit.Router.Transmitted;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramFinancialGameBot.Util.Reply;

namespace TelegramBotShit.Bot;

public class Bot
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    private const int CheckNotificationsDelay = 60000;
    private const int CheckStatisticDelay = 10000;
    
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    public Bot(string apiToken)
    {
        _botClient = new TelegramBotClient(apiToken);
        _cancellationTokenSource = new CancellationTokenSource();

        var botResponder = new BotResponder(_botClient, _cancellationTokenSource);
        
        if (!BotMessageManager.Create(botResponder))
        {
            Logger.Error("Problems with BotMessageManager.Create");
        }
        
        if (!BotNotificationSender.Create(botResponder))
        {
            Logger.Error("Problems with BotNotificationSender.Create");
        }
        
        Logger.Debug("Выполнена инициализация TelegramBotClient");
    }

    public void Start()
    {
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        ChatsRouter chatsRouter = new ChatsRouter();
        BotRequestHandlers botRequestHandlers = new BotRequestHandlers(chatsRouter);

        _botClient.StartReceiving(
            botRequestHandlers.HandleUpdateAsync,
            botRequestHandlers.HandlePollingErrorAsync,
            receiverOptions,
            _cancellationTokenSource.Token
        );
        
        Logger.Debug("Выполнена инициализация ReceiverOptions и BotRequestHandlers и выполнен TelegramBotClient StartReceiving");
        
        BotStatisticManager.GetInstance().StartCollectStatistic(CheckStatisticDelay);

        Logger.Info("Выполнен запуск бота");
    }

    public string GetBotName()
    {
        string? username = _botClient.GetMeAsync().Result.Username;
        
        if (username != null)
        {
            Logger.Debug("Выполнено получение имени бота");
        }
        else
        {
            Logger.Warn("Ошибка получение имени бота");
            username = "";
        }
        
        return username;
    }

    public void Stop()
    {
        Logger.Debug("Начата остановка бота");
        BotNotificationSystem.GetInstance().StopNotificationSystem();

        _cancellationTokenSource.Cancel();
        Logger.Info("Выполнена остановка бота");
    }
}