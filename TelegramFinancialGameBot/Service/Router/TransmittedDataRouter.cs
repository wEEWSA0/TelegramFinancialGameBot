using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TelegramBotShit.Router.Transmitted;

namespace TelegramFinancialGameBot.Service.Router;

public class TransmittedDataRouter
{
    private static TransmittedDataRouter _instance;

    private Dictionary<long, TransmittedData> _chatTransmittedDataPairs;

    private TransmittedDataRouter()
    {
        _chatTransmittedDataPairs = new();
    }

    public static TransmittedDataRouter Instance 
    { 
        get
        {
            if (_instance == null)
            {
                _instance = new TransmittedDataRouter();
            }
            
            return _instance;
        } 
    }

    public TransmittedData GetOrCreateUserTransmittedData(long chatId)
    {
        if (!_chatTransmittedDataPairs.ContainsKey(chatId))
        {
            _chatTransmittedDataPairs[chatId] = new TransmittedData();
        }

        return _chatTransmittedDataPairs[chatId];
    }

    public bool HasUserTransmittedData(long chatId)
        => _chatTransmittedDataPairs.ContainsKey(chatId);
}
