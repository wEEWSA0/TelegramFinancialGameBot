using NLog;
using TelegramFinancialGameBot.Util;

namespace TelegramBotShit.Router.Transmitted;

public class DataStorage
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private Dictionary<string, object> _data;

    public DataStorage()
    {
        _data = new Dictionary<string, object>();
    }

    public void Add(string key, object value)
    {
        _data[key] = value;
    }

    public void Delete(string key)
    {
        _data.Remove(key);
    }

    public void Clear()
    {
        _data.Clear();
    }

    [Obsolete]
    public bool TryGet(string key, out object value)
    {
        value = new object();
        
        if (_data.ContainsKey(key))
        {
            value = _data[key];

            return true;
        }

        return false;
    }

    public bool TryGet<T>(string key, out T value)
    {
        bool res = TryGet(key, out object objValue);

        value = res ? (T)objValue : default;

        return res;
    }

    [Obsolete]
    public object GetOrThrow(string key)
    {
        if (!_data.ContainsKey(key))
        {
            Logger.Error(LoggerUtil.FatalLogicError($"GetOrThrow for {key}"));

            throw new KeyNotFoundException("GetOrThrow");
        }

        return _data[key];
    }

    public T GetOrThrow<T>(string key)
    {
        return (T)GetOrThrow(key);
    }
}