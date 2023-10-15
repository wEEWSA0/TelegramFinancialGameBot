using Telegram.Bot.Types.ReplyMarkups;
using TelegramFinancialGameBot.Util;

namespace TelegramBotShit.Bot;

public class MessageToSend
{
    private const string EmptyMessage = "Empty message";

    public string Text { get; set; }
    public IReplyMarkup? KeyboardMarkup { get; set; }
    //public InputOnlineFile OnlineFile;
    public bool IsLastMessagesHistoryNeeded { get; set; }

    public MessageToSend(string text)
    {
        Text = text;
        IsLastMessagesHistoryNeeded = true;
    }

    public MessageToSend(string text, IReplyMarkup? markup)
    {
        Text = text;
        KeyboardMarkup = markup;
        IsLastMessagesHistoryNeeded = true;
    }

    public MessageToSend(string text, bool isLastMessagesHistoryNeeded)
    {
        Text = text;
        IsLastMessagesHistoryNeeded = isLastMessagesHistoryNeeded;
    }

    public MessageToSend(string text, IReplyMarkup? markup, bool isLastMessagesHistoryNeeded)
    {
        Text = text;
        KeyboardMarkup = markup;
        IsLastMessagesHistoryNeeded = isLastMessagesHistoryNeeded;
    }

    public MessageToSend With(bool isLastMessagesHistoryNeeded)
    {
        IsLastMessagesHistoryNeeded = isLastMessagesHistoryNeeded;

        return this;
    }
    /*
    public MessageToSend(string text, InputOnlineFile onlineFile, bool isLastMessagesHistoryNeeded)
    {
        Text = text;
        IsLastMessagesHistoryNeeded = isLastMessagesHistoryNeeded;
        OnlineFile = onlineFile;
    }

    public MessageToSend(string text, KeyboardMarkup? markup, InputOnlineFile onlineFile, bool isLastMessagesHistoryNeeded)
    {
        Text = text;
        KeyboardMarkup = markup;
        IsLastMessagesHistoryNeeded = isLastMessagesHistoryNeeded;
    }*/

    public static MessageToSend Empty { get => new MessageToSend(EmptyMessage); }
    
    /*
     * Класс, хранящий содержание сообщения, которое будет отправлено
     * Работает с текстом и встроенной в сообщение клавиатурой
     */
}