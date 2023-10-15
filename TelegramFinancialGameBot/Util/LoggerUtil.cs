using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotShit.Router.Transmitted;

namespace TelegramFinancialGameBot.Util;

internal class LoggerUtil
{
    public static string LostServiceMethod(long chatId, TransmittedData transmittedData)
    {
        return $"Ошибка в методе ProcessBotStateToMethod:\n" +
            $"  chatId = {chatId}, состояние = {transmittedData.State}, \n" +
            $"  функция для обработки не найдена, вернется пустое сообщение";
    }

    public static string FoundServiceMethod(long chatId, TransmittedData transmittedData, string methodName)
    {
        return $"Вызван метод ProcessBotStateToMethod:\n" +
            $"  chatId = {chatId}, состояние = {transmittedData.State}, \n" +
            $"  функция для обработки = {methodName}";
    }

    public static string FatalLogicError(string methodName)
    {
        return $"В методе {methodName} произошла ошибка, связанная с логикой взаимодействия методов в программе";
    }
}
