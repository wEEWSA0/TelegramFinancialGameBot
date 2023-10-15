using NLog;

using TelegramBotShit.Bot;

using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Util;
using TelegramFinancialGameBot.Util.Keyboard;
using TelegramFinancialGameBot.Util.Reply;

namespace TelegramFinancialGameBot.Service.Realization;

public class SetupCharacterService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private UserRepository _userRepository;
    private SetupCharacterRepository _characterRepository;

    private PropertyService _propertyService;
    private BuisnessService _buisnessService;
    private StaffService _staffService;
    private WorkService _workService;

    public SetupCharacterService()
    {
        _characterRepository = new();
        _userRepository = new();

        _buisnessService = new();
        _propertyService = new PropertyService();
        _staffService = new StaffService();
        _workService = new WorkService();
    }

    public bool TryGetSetupCharacter(string characterId, out SetupCharacter character)
    {
        return _characterRepository.TryGet(characterId, out character);
    }

    public User CreateUserFromSetupCharacter(long chatId, int roomId, SetupCharacter character)
    {
        User user = new User
        {
            AccountChatId = chatId,
            Cash = character.Cash,
            CashIncome = 0,
            CompleteDream = false,
            FinishedStep = false,
            RoomId = roomId,
            Energy = CharacterDefaultParamters.DefaultEnergy,
            FreeTime = character.FreeTime
        };

        user = _userRepository.AddNew(user);

        foreach (var setupCard in character.Cards)
        {
            var card = setupCard.Card;

            if (_propertyService.TryGetProperty(card, out var property))
            {
                bool usesAsHome = false;
                bool isOwner = false;

                if (setupCard.AdditionalInfo == null)
                {
                    throw new ArgumentNullException(nameof(setupCard));
                }

                if (setupCard.AdditionalInfo == SetupCharacterConstants.OwnAndUsesAsHome)
                {
                    usesAsHome = true;
                    isOwner = true;
                }
                else if (setupCard.AdditionalInfo == SetupCharacterConstants.RentOwnHome)
                {
                    usesAsHome = false;
                    isOwner = true;
                }
                else if (setupCard.AdditionalInfo == SetupCharacterConstants.NotOwnAndRentedHome)
                {
                    usesAsHome = true;
                    isOwner = false;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(setupCard));
                }

                _propertyService.ForceAddProperty(user, property, usesAsHome, isOwner);
                continue;
            }

            if (_buisnessService.TryGetBuisness(card, out var buisness))
            {
                _buisnessService.ForceAddBuisness(user, buisness);
                continue;
            }

            if (_staffService.TryGetStaff(card, out var staff))
            {
                _staffService.ForceAddStaff(user, staff);
                continue;
            }

            if (_workService.TryGetWorkPositionWithZeroExpirience(card, out var position))
            {
                _workService.ForceAddWorkPosition(user, position);
                continue;
            }

            Logger.Error($"Card: {card} not used");
        }

        if (character.Cards != null && character.Cards.Count != 0)
        {
            _userRepository.Update(user);
        }

        return user;
    }
}
