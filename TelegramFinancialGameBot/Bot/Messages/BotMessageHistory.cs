using NLog;
using NLog.Fluent;
using Telegram.Bot.Types;

namespace TelegramBotShit.Bot;

public class BotMessageHistory
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private static BotMessageHistory _messageHistory;
    private List<int> _ordinaryMessagesIds;
    private List<int> _anchoredMessagesIds;

    private BotResponder _botResponder;

    private long _chatId;

    public BotMessageHistory(BotResponder botResponder, long chatId)
    {
        _ordinaryMessagesIds = new List<int>();
        _anchoredMessagesIds = new List<int>();

        _chatId = chatId;
        _botResponder = botResponder;
    }

    public void AddMessageId(int messageId)
    {
        _ordinaryMessagesIds.Add(messageId);
    }

    public void AddAnchoredMessagesId(params int[] messagesId)
    {
        for (int i = 0; i < messagesId.Length; i++)
        {
            _anchoredMessagesIds.Add(messagesId[i]);
        }
    }

    public int AnchoredMessagesCount => _anchoredMessagesIds.Count;

    public void AddMessagesIds(params int[] messagesId)
    {
        for (int i = 0; i < messagesId.Length; i++)
        {
            _ordinaryMessagesIds.Add(messagesId[i]);
        }
    }
    
    public void AddMessageListIds(List<Message> messageList)
    {
        for (int i = 0; i < messageList.Count; i++)
        {
            _ordinaryMessagesIds.Add(messageList[i].MessageId);
        }
    }
    
    public async Task DeleteAllOrdinaryMessages()
    {
        await DeleteMessages(_ordinaryMessagesIds);
        
        _ordinaryMessagesIds.Clear();
    }
    
    public async Task DeleteAllMessages()
    {
        var anchoredMessagesIds = new List<int>(_anchoredMessagesIds);
        
        _anchoredMessagesIds.Clear();
        
        await DeleteAllOrdinaryMessages();
        
        await DeleteMessages(anchoredMessagesIds);
    }
    
    public Task DeleteLastMessage()
    {
        if (_ordinaryMessagesIds.Count < 1)
        {
            throw new Exception("Messages count < 1");
        }

        int lastMessageId = _ordinaryMessagesIds[_ordinaryMessagesIds.Count - 1];
        
        _ordinaryMessagesIds.Remove(lastMessageId);

        return DeleteMessage(lastMessageId);
    }
    
    private Task DeleteMessage(int messageId)
    {
        return _botResponder.DeleteMessage(messageId, _chatId);
    }

    private async Task DeleteMessages(List<int> messageIdList)
    {
       List<int> messageIdsToDelete = new List<int>(messageIdList);
        
        for (int i = 0; i < messageIdsToDelete.Count; i++)
        {
            if (messageIdsToDelete[i] == null)
            {
                Logger.Error("Message to delete equals null! Perhaps that message was deleted earlier");
                
                continue;
            }
            
            try
            {
                await DeleteMessage(messageIdsToDelete[i]);
            }
            catch
            {
                await Console.Out.WriteLineAsync("Exception! Can't delete message");
            }
        }
    }
}