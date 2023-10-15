using System.Linq.Expressions;
using System.Reflection;

using TelegramBotShit.Bot;

using TelegramFinancialGameBot.Service.Router.Transmitted;

namespace TelegramFinancialGameBot.Service.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
internal class BotMethodsClassAttribute : Attribute
{
    //private Dictionary<BotMethodType, Dictionary<int, Func<TransmittedInfo, MessageToSend>>>
    //    keyValuePairsMethods;

    public Dictionary<BotMethodType, Dictionary<int, Func<TransmittedInfo, MessageToSend>>> FindAndGetKeyValuePairs(object targetClass)
    {
        Dictionary<BotMethodType, Dictionary<int, Func<TransmittedInfo, MessageToSend>>> keyValuePairsMethods = new();

        var methods = targetClass.GetType().GetMethods();

        foreach (var m in methods)
        {
            if (IsDefined(m, typeof(BotMethodAttribute)))
            {
                var method = (Func<TransmittedInfo, MessageToSend>)CreateDelegate(m, targetClass);

                if (method == null)
                {
                    throw new NotSupportedException("Wtf?!");
                }

                var attributes = m.GetCustomAttributes<BotMethodAttribute>();
                foreach (var attr in attributes)
                {
                    if (!keyValuePairsMethods.TryGetValue(attr.Type, out Dictionary<int, Func<TransmittedInfo, MessageToSend>> pair))
                    {
                        pair = new();
                    }

                    pair[attr.State] = method;

                    keyValuePairsMethods[attr.Type] = pair;
                }
            }
        }

        return keyValuePairsMethods;
    }

    //public Dictionary<BotMethodType, Dictionary<int, Func<TransmittedInfo, MessageToSend>>> GetKeyValuePairsFromCache()
    //    => keyValuePairsMethods;

    public static Delegate CreateDelegate(MethodInfo methodInfo, object target)
    {
        Func<Type[], Type> getType;
        var isAction = methodInfo.ReturnType.Equals(typeof(void));
        var types = methodInfo.GetParameters().Select(p => p.ParameterType);

        if (isAction)
        {
            getType = Expression.GetActionType;
        }
        else
        {
            getType = Expression.GetFuncType;
            types = types.Concat(new[] { methodInfo.ReturnType });
        }

        if (methodInfo.IsStatic)
        {
            return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);
        }

        return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
    }
}
