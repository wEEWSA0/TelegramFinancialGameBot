using TelegramFinancialGameBot.Service.Router.Transmitted;

namespace TelegramFinancialGameBot.Service.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class BotMethodAttribute : Attribute
{
    public int State { get; init; }
    public BotMethodType Type { get; init; }

    public BotMethodAttribute(int state, BotMethodType methodType)
    {
        State = state;
        Type = methodType;
    }
}
