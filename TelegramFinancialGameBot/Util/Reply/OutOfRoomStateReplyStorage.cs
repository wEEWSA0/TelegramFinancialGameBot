namespace TelegramFinancialGameBot.Util.Reply;

internal class OutOfRoomStateReplyStorage
{
    public readonly string InputYourName =
        "Как мне к Вам обращаться?";

    public string InputUserName(string name) =>
        $"{name}, правильно?";

    public readonly string CreateOrJoinRoom =
        "Создать или войти в комнату?";

    public readonly string InputRoomName =
        "Введите имя комнаты";

    public readonly string RoomAlreadyExist =
        "Такая комната уже существует";

    public readonly string RoomNotFound =
        "Комната не найдена";

    public readonly string FoundRoomWithoutVictoryConditions =
        "Комната найдена, однако она еще настроена создателем комнаты";

    public readonly string ErrorInput =
        "Команда не распознана. Для начала работы с ботом введите /start";

    public readonly string MainMenu =
        "Главное меню";

    public readonly string ArchoredMessage =
        "<b>Что умеет этот бот?</b>\r\n\r\nЯ помощник для настольной игры в “Бирюково: управление капиталом”. Помогаю вам считать, добавлять и удалять карточки из вашего профиля.";

}
