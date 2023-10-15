using TelegramBotShit.Bot;
using TelegramBotShit.Router.Transmitted;

using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Util;

namespace TelegramFinancialGameBot.Service.Realization;

public class AccidentService
{
    private AccidentRepository _accidentRepository;
    private UserRepository _userRepository;

    public AccidentService()
    {
        _userRepository = new();
        _accidentRepository = new();
    }

    public MessageToSend GoToAddMenu(long chatId, TransmittedData data, Accident accident)
    {
        var user = _userRepository.GetByIdOrThrow(data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

        if (accident.Type == AccidentType.Default)
        {
            user.Cash -= accident.Cost;
            user.CashIncome -= accident.CashExpense;
            user.FreeTime -= accident.TimeExpense;
            user.Energy -= accident.EnergyCost;
        }

        _accidentRepository.AddUserAccident(new UserAccident
        {
            AccidentId = accident.Id,
            UserId = user.Id,
            CurrentStep = 0
        });

        _userRepository.Update(user);

        return InRoomService.GoToMainMenuWithRemoveMessageHistory(chatId, data, user, user.Dream, user.Room);
    }

    public bool TryGet(string id, out Accident accident)
    {
        return _accidentRepository.TryGet(id, out accident);
    }

    public List<UserAccident> GetUserAccidentsList(int userId)
    {
        return _accidentRepository.GetUserAccidentsList(userId);
    }

    public List<UserAccident> GetUserAccidentsListWithType(int userId, AccidentType type)
    {
        return _accidentRepository.GetUserAccidentsListWithType(userId, type);
    }

    public void RemoveUserAccident(UserAccident userAccident)
    {
        _accidentRepository.RemoveUserAccident(userAccident);
    }

    public void UpdateUserAccidentList(List<UserAccident> userAccidents)
    {
        _accidentRepository.UpdateUserAccidentList(userAccidents);
    }
}
