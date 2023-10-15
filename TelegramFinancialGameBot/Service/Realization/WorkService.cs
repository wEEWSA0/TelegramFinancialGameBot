using Telegram.Bot.Types.ReplyMarkups;

using TelegramBotShit.Bot;
using TelegramBotShit.Bot.Buttons;
using TelegramBotShit.Router.Transmitted;

using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Service.Attributes;
using TelegramFinancialGameBot.Service.Router.Transmitted;
using TelegramFinancialGameBot.Util;
using TelegramFinancialGameBot.Util.Keyboard;
using TelegramFinancialGameBot.Util.Reply;

namespace TelegramFinancialGameBot.Service.Realization;

[BotMethodsClass]
public class WorkService
{
    private static InRoomStateReplyStorage ReplyText = ReplyStorage.InRoom;
    private static InRoomStateCallbackQueryStorage CallbackData = CallbackQueryStorage.InRoom;

    private UserRepository _userRepository;
    private WorkRepository _workRepository;

    public WorkService()
    {
        _userRepository = new UserRepository();
        _workRepository = new WorkRepository();
    }

    public MessageToSend GoToMenu(long chatId, TransmittedData data)
    {
        var userId = data.Storage.GetOrThrow<int>(ConstantsStorage.UserId);

        var userWorkPositionList = _workRepository.GetUserWorkPositionList(userId);

        data.State = State.InRoom.WorkCategory.Info;

        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

        if (userWorkPositionList.Count == 0)
        {
            return new MessageToSend("У вас нет работы", keyboard, false);
        }

        string text = "";
        var n = 1;

        foreach (var work in userWorkPositionList)
        {
            text += $"""
                
                {n}. {work.WorkPosition.Work.Name}
                Опыт: {work.Experience} ходов
                Требует времени: {work.WorkPosition.RequireTime} часов
                Заработная плата: {work.WorkPosition.Income.ToString("n0")}₽
                """;

            n++;
        }

        if (text == "")
        {
            throw new ArgumentNullException("text");
        }

        return new MessageToSend(text, keyboard, false);
    }


    [BotMethod(State.InRoom.WorkCategory.Info, BotMethodType.Callback)]
    public MessageToSend ProcessMenuButtons(TransmittedInfo info)
    {
        if (info.Request != CallbackData.Back)
        {
            throw new NotImplementedException();
        }

        var userId = info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId);
        var user = _userRepository.GetByIdOrThrow(userId);

        return InRoomService.GoToMainMenu(info.ChatId, info.Data, user, user.Dream, user.Room).With(false);
    }


    public void ForceAddWorkPosition(User user, WorkPosition workPosition)
    {
        _workRepository.AddNew(new UserWorkPosition
        {
            UserId = user.Id,
            WorkPositionId = workPosition.Id,
            Experience = 0
        });

        user.CashIncome += workPosition.Income;
        user.FreeTime -= workPosition.RequireTime;

        _userRepository.Update(user);
    }

    public List<UserWorkPosition> GetUserWorkPositionList(int userId)
    {
        return _workRepository.GetUserWorkPositionList(userId);
    }

    public void UpdateUserWorkPositionList(List<UserWorkPosition> userWorkPositions)
    {
        _workRepository.Update(userWorkPositions);
    }

    public bool TryGetWorkPositionWithZeroExpirience(string workId, out WorkPosition position)
    {
        return _workRepository.TryGetWorkPositionWithZeroExpirience(workId, out position);
    }
}
