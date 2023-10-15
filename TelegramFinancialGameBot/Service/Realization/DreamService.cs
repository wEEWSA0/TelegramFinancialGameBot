using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotShit.Bot.Buttons;
using TelegramBotShit.Bot;
using TelegramBotShit.Router.Transmitted;

using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Service.Router.Transmitted;
using TelegramFinancialGameBot.Util;
using TelegramFinancialGameBot.Util.Keyboard;
using TelegramFinancialGameBot.Util.Reply;
using TelegramBotShit.Service.ServiceRealization;
using Telegram.Bot.Types;

namespace TelegramFinancialGameBot.Service.Realization;

public class DreamService
{
    private UserRepository _userRepository;
    private DreamRepository _dreamRepository;

    public DreamService()
    {
        _userRepository = new();
        _dreamRepository = new();
    }

    public MessageToSend GoToAddMenu(long chatId, TransmittedData data, Dream dream)
    {
        var user = _userRepository.GetByIdOrThrow(data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

        if (user.CompleteDream || _dreamRepository.TryGetUserDreamExpectation(user.Id, out var foundExpectation))
        {
            return new MessageToSend("Мечта уже выбрана");
        }

        var cashDifference = user.Cash - dream.Cost;
        var cashIncomeDifference = user.CashIncome - dream.CashExpense;
        var timeDifference = user.FreeTime - dream.RequireTime;

        if (cashDifference < 0 || cashIncomeDifference < 0 || timeDifference < 0)
        {
            return new MessageToSend("Ошибка выполнения мечты");
        }

        _dreamRepository.AddUserDreamExpectation(new UserDreamExpectation
        {
            DreamId = dream.Id,
            UserId = user.Id,
            Steps = dream.StepCount
        });

        user.DreamId = dream.Id;

        user.Cash -= dream.Cost;
        user.CashIncome -= dream.CashExpense;
        user.FreeTime -= dream.RequireTime;

        _userRepository.Update(user);

        RealizationUtil.AddMessageToStack(chatId, new MessageToSend("Начато выполнение мечты", false));

        return InRoomService.GoToMainMenu(chatId, data, user, dream, user.Room);
    }

    public bool TryGetDream(string code, out Dream dream)
    {
        return _dreamRepository.TryGet(code, out dream);
    }

    public bool TryGetUserDreamExpectation(int userId, out UserDreamExpectation userDreamExpectation)
    {
        return _dreamRepository.TryGetUserDreamExpectation(userId, out userDreamExpectation);
    }

    public void RemoveDreamExpectation(UserDreamExpectation dreamExpectation)
    {
        _dreamRepository.RemoveDreamExpectation(dreamExpectation);
    }

    public void UpdateDreamExpectation(UserDreamExpectation dreamExpectation)
    {
        _dreamRepository.UpdateUserDreamExpectation(dreamExpectation);
    }
}
