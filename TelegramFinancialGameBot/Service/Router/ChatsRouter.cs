using NLog;

using System.Reflection;

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using TelegramBotShit.Bot;
using TelegramBotShit.Router.Transmitted;

using TelegramFinancialGameBot.Service.Attributes;
using TelegramFinancialGameBot.Service.Router;
using TelegramFinancialGameBot.Service.Router.Transmitted;
using TelegramFinancialGameBot.Util;

namespace TelegramBotShit.Router;

public class ChatsRouter
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private Dictionary<BotMethodType, Dictionary<int, Func<TransmittedInfo, MessageToSend>>>
        _keyValuePairsMethods;

    public ChatsRouter()
    {
        _keyValuePairsMethods = new();
        InstanceForKeyValuePairs();
    }

    public MessageToSend RouteMessage(long chatId, Message message)
        => Route(chatId, message.Text, UpdateType.Message);

    public MessageToSend RouteCallbackQuery(long chatId, CallbackQuery callback)
        => Route(chatId, callback.Data, UpdateType.CallbackQuery);

    private MessageToSend Route(long chatId, string data, UpdateType type)
    {
        Logger.Debug($"Старт метода Route в ChatsRouter: chatId = {chatId}");

        TransmittedInfo transmittedInfo = new TransmittedInfo(chatId, TransmittedDataRouter.Instance.GetOrCreateUserTransmittedData(chatId), data);

        MessageToSend messageToSend = ProcessBotStateToMethod(transmittedInfo, type);

        Logger.Debug($"Выполнен метод Route в ChatsRoute: chatId = {chatId}");

        return messageToSend;
    }

    private MessageToSend ProcessBotStateToMethod(TransmittedInfo info, UpdateType type)
    {
        BotMethodType botMethodType = new();

        if (type == UpdateType.Message)
        {
            botMethodType = BotMethodType.Message;
        }
        else if (type == UpdateType.CallbackQuery)
        {
            botMethodType = BotMethodType.Callback;
        }
        else
        {
            Logger.Error("Program logic error (ProcessBotStateToMethod in ChatsRouter)");
            return MessageToSend.Empty;
        }

        if (!_keyValuePairsMethods.ContainsKey(botMethodType) ||
            !_keyValuePairsMethods[botMethodType].ContainsKey(info.Data.State))
        {
            Logger.Error(LoggerUtil.LostServiceMethod(info.ChatId, info.Data));

            return MessageToSend.Empty;
        }

        return _keyValuePairsMethods[botMethodType][info.Data.State](info);
    }

    public void InstanceForKeyValuePairs()
    {
        //var load = 0;

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                //load++;

                var attribs = type.GetCustomAttributes(typeof(BotMethodsClassAttribute), false);

                if (attribs != null && attribs.Length > 0)
                {
                    var instanceOfType = Activator.CreateInstance(type);

                    foreach (BotMethodsClassAttribute attr in (BotMethodsClassAttribute[])attribs)
                    {
                        var pairs = attr.FindAndGetKeyValuePairs(instanceOfType);

                        foreach (var keyType in pairs.Keys)
                        {
                            if (!_keyValuePairsMethods.ContainsKey(keyType))
                            {
                                _keyValuePairsMethods[keyType] = new();
                            }

                            foreach (var state in pairs[keyType].Keys)
                            {
                                if (_keyValuePairsMethods[keyType].ContainsKey(state))
                                {
                                    Logger.Warn($"Key value pair already exist. Duplicate method for {keyType}!\n" +
                                    $"  Methods: {pairs[keyType][state].Method.Name}, {_keyValuePairsMethods[keyType][state].Method.Name}");
                                }

                                _keyValuePairsMethods[keyType][state] = pairs[keyType][state];
                            }
                        }
                            //bool isAdded = _keyValuePairsMethods.TryAdd(tp.Key, tp.Value);

                            //if (!isAdded)
                            //{
                            //    Logger.Warn($"Key value pair already exist. Duplicate method for {tp.Key}!\n" +
                            //        $"  Methods: {tp.Value}, {_keyValuePairsMethods[tp.Key]}");
                            //}
                    }
                }
            }
        }

        //Console.WriteLine(load);
    }
}