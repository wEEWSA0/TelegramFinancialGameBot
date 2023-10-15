using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TelegramBotShit.Bot;
using TelegramBotShit.Router.Transmitted;
using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Service.Router.Transmitted;
using TelegramFinancialGameBot.Util.Keyboard;
using TelegramFinancialGameBot.Util;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Service.Realization;
public static class RealizationUtil
{
    public static MessageToSend ProcessButtons(TransmittedInfo info, params CallbackMethod[] callbackMethods)
    {
        var callbackMethod = callbackMethods.FirstOrDefault(c => c.Callback == info.Request);

        if (callbackMethod != null)
        {
            return callbackMethod.Func.Invoke(info.ChatId, info.Data);
        }
        else
        {
            return MessageToSend.Empty;
        }
    }

    public static void AddMessageToStack(long chatId, MessageToSend messageToSend)
    {
        BotMessageManager.Instance.GetSender(chatId).AddMessageToStack(messageToSend);
    }

    public static void DeleteLastMessage(long chatId)
    {
        BotMessageManager.Instance.GetHistory(chatId).DeleteLastMessage();
    }
}

public class CallbackMethod
{
    public string Callback { get; set; }
    public Func<long, TransmittedData, MessageToSend> Func { get; set; }

    public CallbackMethod(string callback, Func<long, TransmittedData, MessageToSend> func)
    {
        Callback = callback;
        Func = func;
    }
}
