using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotShit.Router.Transmitted;

namespace TelegramFinancialGameBot.Service.Router.Transmitted;

public class TransmittedInfo
{
    public long ChatId { get; private set; }
    public TransmittedData Data { get; private set; }
    public string Request { get; private set; }

    public TransmittedInfo(long chatId, TransmittedData transmittedData, string request)
    {
        ChatId = chatId;
        Data = transmittedData;
        Request = request;
    }

    public TransmittedInfo GetNewWithRequest(string request)
    {
        return new TransmittedInfo(ChatId, Data, request);
    }
}
