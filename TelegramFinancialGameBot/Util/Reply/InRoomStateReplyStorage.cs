using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Util.Reply;

internal class InRoomStateReplyStorage
{
    public readonly string InputCharacterCode =
        "Введите код вашего персонажа";

    //public readonly string InputDreamCode =
    //    "Введите код вашей мечты";

    public readonly string IncorrentCode =
        "Код введен некорректно";

    public readonly string ChooseCatalog =
        "Какой раздел открыть?";

    public readonly string ChooseCategoryToOpen =
        "Выберите категорию:";

    public string JoinInRoom(string roomName, VictoryConditions conditions, User user)
    {
        // green 🟢
        // red 🔴
        // empty ⚪️

        var dreamSub = user.CompleteDream ? "🟢" : "🔴";
        string dream = conditions.ShouldDreamBeCompleted ? $"{dreamSub} Мечта" : "";

        var cashIncome = user.CashIncome - conditions.CashIncome < 0 ? "🔴" : "🟢";
        var freeTime = user.FreeTime - conditions.RequireTime < 0 ? "🔴" : "🟢";

        return $"Вы вошли в комнату {roomName} сообщите другим игрокам название комнаты\r\n\r\n" +
        $"Условия победы\r\n" +
        $"{cashIncome} Денежный поток = {conditions.CashIncome.ToString("n0")}₽\r\n" +
        $"{freeTime} Свободное время = {conditions.RequireTime.ToString("n0")}ч\r\n" +
        //$"⚪️ Банковские счета >= Расход за {conditions.TimeForPaymentsToBank.ToString("n")} мес.\r\n" +
        $"{dream}";
    }

    public string FirstJoinInRoom(string roomName, VictoryConditions conditions)
    {
        // green 🟢
        // red 🔴
        // empty ⚪️

        string dream = conditions.ShouldDreamBeCompleted ? $"⚪️ Мечта" : "";

        return $"Вы вошли в комнату {roomName} сообщите другим игрокам название комнаты\r\n\r\n" +
        $"Условия победы\r\n" +
        $"⚪️ Денежный поток = {conditions.CashIncome.ToString("n0")}₽\r\n" +
        $"⚪️ Свободное время = {conditions.RequireTime.ToString("n0")}ч\r\n" +
        //$"⚪️ Банковские счета >= Расход за {conditions.TimeForPaymentsToBank.ToString("n")} мес.\r\n" +
        $"{dream}";
    }

    public string Profile(string name, User user, Dream? dream, short step)
    {
        var dreamText = dream is null ? $"Мечта: не выбрана" : $"Мечта: {dream.Name}";

        return $"""
                {name}, ваши данные

                Наличные: {user.Cash.ToString("n0")}₽
                Денежный поток: {user.CashIncome.ToString("n0")}₽/мес
                Свободное время: {user.FreeTime.ToString("n0")} ч/мес
                {dreamText}

                Доступно: {user.Energy.ToString("n0")} энергии
                Ход: {step}
                """;
    }

    // todo Добавить имена-ссылки
    public string PlayersStatistic(List<User> users, VictoryConditions conditions)
    {
        var res = "";
        // TODO корона у лучших
        foreach (var user in users)
        {
            var freeTime = user.FreeTime >= conditions.RequireTime ? "🟢" : "🔴";
            var cashIncome = user.CashIncome >= conditions.CashIncome ? "🟢" : "🔴";

            if (conditions.ShouldDreamBeCompleted)
            {
                var isDreamCompleted = user.CompleteDream ? "🟢" : "🔴";

                res += $"""
                    {user.Account.Name}
                    {cashIncome} Денежный поток
                    {freeTime} Свободное время
                    {isDreamCompleted} Мечта
                
                    """;
            }
            else
            {
                res += $"""
                    {user.Account.Name}
                    {cashIncome} Денежный поток
                    {freeTime} Свободное время
                
                    """;
            }

            res += "\n";
        }

        if (res == "")
        {
            res = "Ошибка загрузки";
        }

        return res;
    }

    // Categories
    public readonly string YourProperties =
        "Ваша недвижимость:";

    public readonly string ChoosePropertiesToMoveIn =
        "Выберите недвижимость для переезда:";

    public string LeftProperty(bool isRented, string newPropertyName)
        => isRented ? $"Вы съехали с арендованной недвижимости в {newPropertyName}"
        : $"Вы съехали с недвижимости {newPropertyName}";

    public string MoveToFirstProperty(bool isRented, string newPropertyName)
        => isRented ? $"Вы въехали в арендованную недвижимость {newPropertyName}"
        : $"Вы въехали в {newPropertyName}";

    public string PropertySaled(string propertyName, int propertyCost)
        => $"Недвижимость - {propertyName} продана за {propertyCost.ToString("n0")}₽";

    public string PropertyInfo(UserProperty userProperty)
    {
        var property = userProperty.Property;

        return userProperty.IsOwner ? 
            !userProperty.UsesAsHome ? $"""
            {property.Name} (Сдаете)

            Стоимость объекта: {property.Cost}₽
            Сдача в аренду: {property.RentCashIncome}₽
            Постоянные расходы: {property.CashExpense}₽
            Требует времени: {property.TimeExpense}ч

            Постоянные доход: {property.RentCashIncome}₽
            """
            :
            $"""
            {property.Name} (Место проживания)

            Стоимость объекта: {property.Cost}₽
            Постоянные расходы: {property.CashExpense}₽
            Требует времени: {property.TimeExpense}ч
            """
            :
            $"""
            {property.Name} (Арендуете)

            Аренда: {property.RentCashIncome}₽
            Требует времени: {property.TimeExpense}ч
            """;
    }

    public readonly string YourBuisnesses =
        "Ваши бизнессы:";

    public readonly string BuisnessesSaleType =
        "Хотите продать весь бизнес или укажите кол-во филиал";

    public string BuisnessInfo(UserBuisness userBuisness)
    {
        var buisness = userBuisness.Buisness;

        return $"""
            {buisness.Name}

            Показатели сети
            Требует времени: {(buisness.RequireTime * userBuisness.BranchCount).ToString("n0")} часа
            Постоянные доходы: {(buisness.CashIncome * userBuisness.BranchCount).ToString("n0")}₽
            Постоянные расходы: {(buisness.CashExpense * userBuisness.BranchCount).ToString("n0")}₽
            Кол-во филиалов: {userBuisness.BranchCount}

            Стоимость: {(buisness.Cost * userBuisness.BranchCount).ToString("n0")}₽
            Доход: {((buisness.CashIncome - buisness.CashExpense) * userBuisness.BranchCount).ToString("n0")}₽
            """;
    }

    public string BuisnessSale(UserBuisness userBuisness, int percent, int finalCost)
    {
        var buisness = userBuisness.Buisness;

        return $"""
            Рынок готов купить за {percent}%:

            Бизнес - {buisness.Name}
            Кол-во филиалов: {userBuisness.BranchCount}
            Стоимость: {(buisness.Cost * userBuisness.BranchCount).ToString("n0")}₽
            Доход: {((buisness.CashIncome - buisness.CashExpense) * userBuisness.BranchCount).ToString("n0")}₽

            <b>Продать за {finalCost.ToString("n0")}₽?</b>
            """;
    }

    public string PurchaseFail(int? difCash, int? difTime, int? difIncome, int? difEnergy)
    {
        var text = "Ошибка покупки";

        if (difCash != null)
        {
            int value = (int)difCash;
            var icon = value >= 0 ? "🟢" : "🔴";
            text += $"\n{icon}Наличных: {Math.Abs(value).ToString("n0")}₽";
        }

        if (difTime != null)
        {
            int value = (int)difTime;
            var icon = value >= 0 ? "🟢" : "🔴";
            text += $"\n{icon}Свободное время: {Math.Abs(value).ToString("n0")}ч";
        }

        if (difIncome != null)
        {
            int value = (int)difIncome;
            var icon = value >= 0 ? "🟢" : "🔴";
            text += $"\n{icon}Денежный поток: {Math.Abs(value).ToString("n0")}₽";
        }

        if (difEnergy != null)
        {
            int value = (int)difEnergy;
            var icon = value >= 0 ? "🟢" : "🔴";
            text += $"\n{icon}Энергия: {Math.Abs(value).ToString("n0")}";
        }

        return text;
    }
}
