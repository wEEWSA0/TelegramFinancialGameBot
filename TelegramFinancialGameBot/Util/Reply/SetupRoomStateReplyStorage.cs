using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Util.Reply;

internal class SetupRoomStateReplyStorage
{
    public string SetupVictoryConditions(VictoryConditions conditions)
    {
        string dream = conditions.ShouldDreamBeCompleted ? "Вкл" : "Выкл";

        return $"<b>Установите условия победы</b>\r\n\r\n" +
        $"Денежный поток = {conditions.CashIncome.ToString("n0")}₽\r\n" +
        $"Свободное время = {conditions.RequireTime.ToString("n0")}ч\r\n" +
        //$"Подушка безопасности = {conditions.TimeForPaymentsToBank} мес.\r\n" +
        $"Мечта = {dream}";
    }

    public readonly string ChangeCashIncome =
        "Укажите денежный поток";

    public readonly string ChangeCashIncomeError =
        "Укажите денежный поток в пределах от 0 до 1 000 000 000";

    public readonly string ChangeFreeTime =
        "Укажите свободное время";

    public readonly string ChangeFreeTimeError =
        "Укажите денежный поток в пределах от 0 до 1 000";

    //public readonly string ChangeFreeTime =
    //    "Укажите свободное время";

    //public readonly string ChangeFreeTimeError =
    //    "Укажите денежный поток в пределах от 0 до 1 000";
}
