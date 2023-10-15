using Telegram.Bot.Types.ReplyMarkups;

using TelegramBotShit.Bot;
using TelegramBotShit.Bot.Buttons;
using TelegramBotShit.Router.Transmitted;
using TelegramBotShit.Service.ServiceRealization;

using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Service.Attributes;
using TelegramFinancialGameBot.Service.Router;
using TelegramFinancialGameBot.Service.Router.Transmitted;
using TelegramFinancialGameBot.Util;
using TelegramFinancialGameBot.Util.Keyboard;
using TelegramFinancialGameBot.Util.Reply;

namespace TelegramFinancialGameBot.Service.Realization;

[BotMethodsClass]
public class InRoomService
{
    private static InRoomStateReplyStorage ReplyText = ReplyStorage.InRoom;
    private static InRoomStateCallbackQueryStorage CallbackData = CallbackQueryStorage.InRoom;

    private RoomRepository _roomRepository;
    private UserRepository _userRepository;

    private PropertyService _propertyService;
    private WorkService _workService;
    private BuisnessService _buisnessService;
    private StaffService _staffService;
    private SetupCharacterService _setupCharacterService;
    private DreamService _dreamService;
    private AccidentService _accidentService;

    public InRoomService()
    {
        _roomRepository = new RoomRepository();
        _userRepository = new UserRepository();

        _propertyService = new PropertyService();
        _workService = new WorkService();
        _buisnessService = new BuisnessService();
        _staffService = new StaffService();
        _setupCharacterService = new SetupCharacterService();
        _dreamService = new DreamService();
        _accidentService = new AccidentService();
    }

    [BotMethod(State.InRoom.SetupCharacter.CreateCharacter, BotMethodType.Message)]
    public MessageToSend ProcessInputCharacter(TransmittedInfo info)
    {
        if (!_setupCharacterService.TryGetSetupCharacter(info.Request, out SetupCharacter character))
        {
            return new MessageToSend(ReplyText.IncorrentCode);
        }

        var roomId = info.Data.Storage.GetOrThrow<int>(ConstantsStorage.RoomId);
        var user = _setupCharacterService.CreateUserFromSetupCharacter(info.ChatId, roomId, character);

        info.Data.Storage.Add(ConstantsStorage.UserId, user.Id);

        return SetuppedGoToMainMenu(info, user);
    }

    
    [BotMethod(State.InRoom.MainMenu, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForMainMenu(TransmittedInfo info)
    {
        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackData.Catalog, GoToCatalog),
            new (CallbackData.PlayersStatistic, ProcessButtonPlayersStatistic),
            new (CallbackData.ExitRoom, ProcessButtonExit)
        });

        MessageToSend ProcessButtonPlayersStatistic(long chatId, TransmittedData data)
        {
            data.State = State.InRoom.PlyersInRoomStatistic;

            if (!_userRepository.TryGetUsersInRoom(data.Storage.GetOrThrow<int>(ConstantsStorage.RoomId), out List<User> users))
            {
                throw new Exception();
            }

            var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back)
                );

            return new MessageToSend(ReplyText.PlayersStatistic(users, users[0].Room.VictoryConditions), keyboard, false);
        }

        MessageToSend ProcessButtonExit(long chatId, TransmittedData data)
        {
            var roomId = data.Storage.GetOrThrow<int>(ConstantsStorage.RoomId);

            if (!_userRepository.TryGetUsersInRoomWithoutRoom(roomId, out List<User> users))
            {
                throw new NotSupportedException("" + roomId);
            }

            var user = users.Where(u => u.AccountChatId == chatId).First();

            _userRepository.Remove(user);

            var room = _roomRepository.GetByIdOrThrow(data.Storage.GetOrThrow<int>(ConstantsStorage.RoomId));

            if (users.Count <= 1)
            {
                _roomRepository.Remove(room);
            }

            return OutOfRoomService.GoToCreateOrJoinRoom(data);
        }
    }

    
    [BotMethod(State.InRoom.MainMenu, BotMethodType.Message)]
    public MessageToSend ProcessReplyButtonsForMainMenu(TransmittedInfo info)
    {
        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new ("Открыть карту", GoToOpenCard),
            new ("Ход", ProcessButtonStep)
        });
        // TODO проверить сообщения на множестве игроков (штрафы, еда)

        MessageToSend ProcessButtonStep(long chatId, TransmittedData data)
        {
            var roomId = data.Storage.GetOrThrow<int>(ConstantsStorage.RoomId);
            
            if (!_userRepository.TryGetUsersInRoomWithoutRoom(roomId, out var users))
            {
                throw new ArgumentNullException(nameof(roomId));
            }

            var room = _roomRepository.GetByIdOrThrow(roomId);

            User myUser = new User();

            bool isCompletedStep = true;
            foreach ( var user in users )
            {
                if (user.AccountChatId == chatId)
                {
                    myUser = user;

                    continue;
                }

                if (user.FinishedStep == false)
                {
                    isCompletedStep = false;

                    continue;
                }
            }

            if (myUser == new User())
            {
                throw new Exception("WTF?");
            }

            if (!isCompletedStep)
            {
                myUser.FinishedStep = true;

                _userRepository.Update(myUser);

                return new MessageToSend("Вы подтвердили готовность к завершению хода. Ожидайте оставшихся игроков");
            }

            Dictionary<long, List<MessageToSend>> messagesToSendAfterStepCompleted = new();

            // При завершении хода всеми игроками
            foreach (var user in users)
            {
                user.FinishedStep = false;

                var messagesToSendForUser = new List<MessageToSend>();

                // Расчеты до хода
                CalculateBuisnessAccident(user);
                CalculateWorkAccident(user);

                var earnedCash = user.CashIncome;

                #region Food
                var foodExpense = 0;

                if (earnedCash < CharacterDefaultParamters.CashForRich)
                {
                    if (earnedCash > 0)
                    {
                        foodExpense = (int)Math.Round(earnedCash * (CharacterDefaultParamters.FoodExpensePercentForNotRich / (double)100));
                    }
                }
                else
                {
                    foodExpense = CharacterDefaultParamters.FoodExpenseForRich;
                }

                earnedCash -= foodExpense;

                messagesToSendForUser.Add(new MessageToSend($"Вы потратили на еду {foodExpense.ToString("n0")}₽"));
                #endregion

                user.Cash += earnedCash;

                // Ход
                user.Energy = CharacterDefaultParamters.DefaultEnergy;
                user.CashIncome = 0;

                // Расчеты после ходы
                CalculateDefaultAccident(user);
                var hasHome = CalculatePropertyAfterSayHasUserHome(user);
                CalculateWork(user, true);
                CalculateAllStaff(user);
                CalculateBuisness(user, 100);
                CalculateDream(user);

                #region PenaltyForHomeless
                if (!hasHome)
                {
                    var fine = CharacterDefaultParamters.PenaltyForHomeless;

                    user.Cash -= fine;

                    messagesToSendForUser.Add(new MessageToSend($"Вы были оштрафованы на {fine.ToString("n0")}₽ за остутствие места жительства"));
                }
                #endregion

                messagesToSendAfterStepCompleted[user.AccountChatId] = messagesToSendForUser;
            }

            room.Step++;

            _userRepository.UpdateRange(users.ToArray());

            _roomRepository.Update(room);

            var messageAboutFinishedStep = new MessageToSend("Ход завершен", false);

            // Отправка сообщения о завершении хода и начала нового
            foreach (var user in users)
            {
                var sender = BotMessageManager.Instance.GetSender(user.AccountChatId);

                sender.AddMessageToStack(messageAboutFinishedStep);

                // Сообщение как минимум о еде будет
                var messagesToSend = messagesToSendAfterStepCompleted[user.AccountChatId];

                foreach (var mes in messagesToSend)
                {
                    sender.AddMessageToStack(mes);
                }

                if (user.AccountChatId == myUser.AccountChatId)
                {
                    continue;
                }

                TransmittedData transmittedData = TransmittedDataRouter.Instance.GetOrCreateUserTransmittedData(user.AccountChatId);

                sender.AddMessageToStack(GoToMainMenu(user.AccountChatId, transmittedData, user, user.Dream, room).With(true));

                sender.SendAllMessages().ConfigureAwait(false);
            }

            return GoToMainMenu(chatId, data, myUser, myUser.Dream, room).With(true);
        }

        bool CalculatePropertyAfterSayHasUserHome(User user)
        {
            var propertyList = _propertyService.GetUserPropertyList(user.Id);
            bool hasHome = false;

            foreach (var property in propertyList)
            {
                if (property.IsOwner)
                {
                    if (property.UsesAsHome)
                    {
                        hasHome = true;
                        user.CashIncome -= property.Property.CashExpense;
                    }
                    else
                    {
                        user.CashIncome += property.Property.RentCashIncome;
                    }
                }
                else
                {
                    hasHome = true;
                    user.CashIncome -= property.Property.RentCashIncome;
                }
            }

            return hasHome;
        }

        void CalculateWork(User user, bool addExpirience)
        {
            var works = _workService.GetUserWorkPositionList(user.Id);

            foreach (var work in works)
            {
                if (addExpirience)
                {
                    work.Experience++;
                }

                user.CashIncome += work.WorkPosition.Income;
            }

            _workService.UpdateUserWorkPositionList(works);
        }

        void CalculateAllStaff(User user)
        {
            #region ManagerStaff
            var staffList = _staffService.GetStaffList(user.Id);

            foreach (var staff in staffList)
            {
                user.CashIncome -= staff.CashExpense;
            }
            #endregion
            #region ManagerStaff
            var managerStaffList = _staffService.GetManagerStaffList(user.Id);

            foreach (var staff in managerStaffList)
            {
                user.CashIncome -= staff.CashExpense;
            }
            #endregion
            #region RegionalDirector
            var regionalDirectorList = _staffService.GetRegionalDirectorList(user.Id);

            foreach (var staff in regionalDirectorList)
            {
                user.CashIncome -= staff.CashExpense;
            }
            #endregion
            #region FinancialDirector
            var financialDirectorList = _staffService.GetFinancialDirectorList(user.Id);

            foreach (var staff in financialDirectorList)
            {
                user.CashIncome -= staff.CashExpense;
            }
            #endregion
            #region GeneralDirector
            var generalDirectorList = _staffService.GetGeneralDirectorList(user.Id);

            foreach (var staff in generalDirectorList)
            {
                user.CashIncome -= staff.CashExpense;
            }
            #endregion
        }

        void CalculateBuisness(User user, short percentOverProfit)
        {
            var buisnesses = _buisnessService.GetUserBuisnessList(user.Id);

            foreach (var buisness in buisnesses)
            {
                buisness.OpenSteps++;

                var buisnessIncome = 0;

                if (buisness.OpenSteps == 0)
                {
                    buisnessIncome = buisness.Buisness.CashIncome - (int)Math.Round(
                        buisness.Buisness.CashIncome *
                        (buisness.Buisness.VariableExpenses / (double)100), MidpointRounding.AwayFromZero);
                }

                if (buisness.OpenSteps > 0)
                {
                    var income = buisness.Buisness.CashIncome - (int)Math.Round(
                        buisness.Buisness.CashIncome *
                        (buisness.Buisness.VariableExpenses / (double)100), MidpointRounding.AwayFromZero);

                    int incomeBuffPercent = 0;

                    if (buisness.FinancialDirectorId != null)
                    {
                        incomeBuffPercent += buisness.FinancialDirector.CashIncomePercent;
                    }

                    if (buisness.GeneralDirectorId != null)
                    {
                        incomeBuffPercent += buisness.GeneralDirector.CashIncomePercent;
                    }

                    buisnessIncome = (int)Math.Round(income * (double)(100 + incomeBuffPercent) / 100);
                }

                if (percentOverProfit != 100 && buisness.OpenSteps >= 0)
                {
                    buisnessIncome = (int)Math.Round(buisnessIncome * (double)percentOverProfit / 100);
                }

                user.CashIncome += buisnessIncome;
            }

            _buisnessService.UpdateUserBuisnessList(buisnesses);
        }

        void CalculateDream(User user)
        {
            if (user.DreamId is not null)
            {
                if (_dreamService.TryGetUserDreamExpectation(user.Id, out var dreamExpectation))
                {
                    var dream = dreamExpectation.Dream;

                    dreamExpectation.Steps--;

                    user.Cash -= dream.Cost;
                    user.CashIncome -= dream.CashExpense;

                    if (dreamExpectation.Steps <= 0)
                    {
                        user.FreeTime += dream.RequireTime;
                        user.CompleteDream = true;

                        _dreamService.RemoveDreamExpectation(dreamExpectation);
                    }
                    else
                    {
                        _dreamService.UpdateDreamExpectation(dreamExpectation);
                    }
                }
            }
        }

        void CalculateDefaultAccident(User user)
        {
            var userAccidents = _accidentService.GetUserAccidentsListWithType(user.Id, AccidentType.Default);

            foreach (var userAccident in userAccidents.ToList())
            {
                var accident = userAccident.Accident;

                userAccident.CurrentStep++;

                if (userAccident.CurrentStep >= userAccident.Accident.StepsDuration)
                {
                    _accidentService.RemoveUserAccident(userAccident);
                    userAccidents.Remove(userAccident);
                }
                else
                {
                    user.CashIncome -= accident.CashExpense;
                    user.FreeTime -= accident.TimeExpense;
                    user.Energy -= accident.EnergyCost;
                }
            }

            _accidentService.UpdateUserAccidentList(userAccidents);
        }

        void CalculateBuisnessAccident(User user)
        {
            var userAccidents = _accidentService.GetUserAccidentsListWithType(user.Id, AccidentType.BuisnessProfitPercent);

            foreach (var userAccident in userAccidents.ToList())
            {
                var accident = userAccident.Accident;

                if (userAccident.CurrentStep >= userAccident.Accident.StepsDuration)
                {
                    _accidentService.RemoveUserAccident(userAccident);
                    userAccidents.Remove(userAccident);
                }
                else
                {
                    if (accident.CashExpense != 0)
                    {
                        CalculateBuisness(user, (short)-accident.CashExpense);
                    }
                    
                    if (accident.Cost != 0 && userAccident.CurrentStep == 0)
                    {
                        CalculateBuisness(user, (short)-accident.Cost);
                    }

                    user.FreeTime -= accident.TimeExpense;
                    user.Energy -= accident.EnergyCost;
                }

                userAccident.CurrentStep++;
            }

            _accidentService.UpdateUserAccidentList(userAccidents);
        }

        void CalculateWorkAccident(User user)
        {
            var userAccidents = _accidentService.GetUserAccidentsListWithType(user.Id, AccidentType.WorkSalary);

            foreach (var userAccident in userAccidents.ToList())
            {
                var accident = userAccident.Accident;

                if (userAccident.CurrentStep >= userAccident.Accident.StepsDuration)
                {
                    _accidentService.RemoveUserAccident(userAccident);
                    userAccidents.Remove(userAccident);
                }
                else
                {
                    if (accident.Cost != 0 && userAccident.CurrentStep == 0)
                    {
                        if (accident.Cost > 1)
                        {
                            for (int i = 0; i < accident.Cost; i++)
                            {
                                CalculateWork(user, false);
                            }
                        }
                        else
                        {
                            CalculateWork(user, false);
                        }
                    }

                    user.FreeTime -= accident.TimeExpense;
                    user.Energy -= accident.EnergyCost;
                }

                userAccident.CurrentStep++;
            }

            _accidentService.UpdateUserAccidentList(userAccidents);
        }
    }


    [BotMethod(State.InRoom.Catalog, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForCatalog(TransmittedInfo info)
    {
        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackData.Property, _propertyService.GoToMenu),
            new (CallbackData.Work, _workService.GoToMenu),
            new (CallbackData.Buisness, _buisnessService.GoToMenu),
            new (CallbackData.Staff, StaffService.GoToMenu),
            new (CallbackData.Back, ProcessButtonBackSub)
        });

        MessageToSend ProcessButtonBackSub(long chatId, TransmittedData data)
        {
            return ProcessButtonBack(new TransmittedInfo(chatId, data, "none"));
        }
    }


    [BotMethod(State.InRoom.OpenCardMenu, BotMethodType.Message)]
    public MessageToSend ProcessInputForOpenCardMenu(TransmittedInfo info)
    {
        if (_propertyService.TryGetProperty(info.Request, out var property))
        {
            return _propertyService.GoToAddMenu(info.ChatId, info.Data, property);
        }

        if (_buisnessService.TryGetBuisness(info.Request, out var buisness))
        {
            return _buisnessService.GoToAddMenu(info.ChatId, info.Data, buisness);
        }

        if (_staffService.TryGetStaff(info.Request, out var staff))
        {
            return _staffService.GoToAddMenu(info.ChatId, info.Data, staff);
        }

        if (_staffService.TryGetManagerStaff(info.Request, out var manager))
        {
            return _staffService.GoToAddMenu(info.ChatId, info.Data, manager);
        }

        if (_staffService.TryGetRegionalDirector(info.Request, out var regDirector))
        {
            return _staffService.GoToAddMenu(info.ChatId, info.Data, regDirector);
        }

        if (_staffService.TryGetFinancialDirector(info.Request, out var finDirector))
        {
            return _staffService.GoToAddMenu(info.ChatId, info.Data, finDirector);
        }

        if (_staffService.TryGetGeneralDirector(info.Request, out var genDirector))
        {
            return _staffService.GoToAddMenu(info.ChatId, info.Data, genDirector);
        }

        if (_dreamService.TryGetDream(info.Request, out var dream))
        {
            return _dreamService.GoToAddMenu(info.ChatId, info.Data, dream);
        }

        if (_accidentService.TryGet(info.Request, out var accident))
        {
            return _accidentService.GoToAddMenu(info.ChatId, info.Data, accident);
        }

        return new MessageToSend("Код не распознан");
    }


    [BotMethod(State.InRoom.OpenCardMenu, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForOpenCardMenu(TransmittedInfo info)
    {
        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackData.Property, ProcessButtonProperty),
            new (CallbackData.Buisness, ProcessButtonBuisness),
            new (CallbackData.Staff, ProcessButtonStaff),
            new (CallbackData.Dream, ProcessButtonDream),
            new (CallbackData.Accident, ProcessButtonAccident),
            new (CallbackData.Back, ProcessButtonBackSub)
        });

        MessageToSend ProcessButtonBackSub(long chatId, TransmittedData data)
        {
            return ProcessButtonBack(new TransmittedInfo(chatId, data, "none"));
        }

        MessageToSend ProcessButtonProperty(long chatId, TransmittedData data)
        {
            return new MessageToSend("Введите номер карты недвижимости (NMxx, NSxx, NKxx)");
        }

        MessageToSend ProcessButtonBuisness(long chatId, TransmittedData data)
        {
            return new MessageToSend("Введите номер карты бизнеса (BMxx)");
        }

        MessageToSend ProcessButtonStaff(long chatId, TransmittedData data)
        {
            return new MessageToSend("Введите номер карты персонала");
        }

        MessageToSend ProcessButtonDream(long chatId, TransmittedData data)
        {
            return new MessageToSend("Введите номер карты мечты (MCxx)");
        }

        MessageToSend ProcessButtonAccident(long chatId, TransmittedData data)
        {
            return new MessageToSend("Введите номер карты события (CXxx)");
        }
    }


    [BotMethod(State.InRoom.PlyersInRoomStatistic, BotMethodType.Callback)]
    [BotMethod(State.InRoom.Profile, BotMethodType.Callback)]
    public MessageToSend ProcessButtonBack(TransmittedInfo info)
    {
        //if (!_userRepository.TryGet(info.ChatId, info.Data.Storage.GetOrThrow<int>(ConstantsStorage.RoomId), out User user))
        //{
        //    throw new Exception();
        //}

        var user = _userRepository.GetByIdOrThrow(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

        return GoToMainMenuWithRemoveMessageHistory(info.ChatId, info.Data, user, user.Dream, user.Room);
    }

    #region SupportMethods

    private MessageToSend SetuppedGoToMainMenu(TransmittedInfo info, User user)
    {
        if (!_roomRepository.TryGet(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.RoomId), out Room room))
        {
            throw new NotImplementedException(nameof(SetuppedGoToMainMenu));
        }

        return GoToMainMenu(info.ChatId, info.Data, user, user.Dream, room);
    }

    public static MessageToSend GoToMainMenu(long chatId, TransmittedData data, User user, Dream? dream, Room room)
    {
        return GoToMainMenu(chatId, data, user, dream, room, true);
    }

    public static MessageToSend GoToMainMenuWithRemoveMessageHistory(long chatId, TransmittedData data, User user, Dream? dream, Room room)
    {
        return GoToMainMenu(chatId, data, user, dream, room, false);
    }

    private static MessageToSend GoToMainMenu(long chatId, TransmittedData data, User user, Dream? dream, Room room, bool isLastMessageHistoryNedeed)
    {
        data.State = State.InRoom.MainMenu;

        var name = data.Storage.GetOrThrow<string>(ConstantsStorage.AccountName);

        // Reply клавиатура
        KeyboardButton[][] buttons =
        {
            new KeyboardButton[]
            {
                new KeyboardButton("Открыть карту"),
                new KeyboardButton("Ход")
            }/*,
            new KeyboardButton[]
            {
                new KeyboardButton("Профиль")
            }*/
        };

        var replyKeyboard = new ReplyKeyboardMarkup(buttons);
        replyKeyboard.ResizeKeyboard = true;

        var mes = new MessageToSend(ReplyText.JoinInRoom(room.Name, room.VictoryConditions, user), replyKeyboard, isLastMessageHistoryNedeed);

        RealizationUtil.AddMessageToStack(chatId, mes);

        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Каталог", CallbackData.Catalog),
            InlineKeyboardButton.WithCallbackData("Статистика по игрокам", CallbackData.PlayersStatistic),
            InlineKeyboardButton.WithCallbackData("Выйти из комнаты", CallbackData.ExitRoom)
            );

        return new MessageToSend(ReplyText.Profile(name, user, dream, room.Step), keyboard);
    }

    public static MessageToSend GoToCatalog(long chatId, TransmittedData data)
    {
        data.State = State.InRoom.Catalog;

        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Бизнес", CallbackData.Buisness),
            InlineKeyboardButton.WithCallbackData("Недвижимость", CallbackData.Property),
            //InlineKeyboardButton.WithCallbackData("Транспорт", CallbackData.Car),
            InlineKeyboardButton.WithCallbackData("Мой персонал", CallbackData.Staff),
            InlineKeyboardButton.WithCallbackData("Моя занятость", CallbackData.Work),
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back)
            );

        return new MessageToSend(ReplyText.ChooseCatalog, keyboard, false);
    }

    public static MessageToSend GoToOpenCard(long chatId, TransmittedData data)
    {
        data.State = State.InRoom.OpenCardMenu;

        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Бизнес", CallbackData.Buisness),
            InlineKeyboardButton.WithCallbackData("Недвижимость", CallbackData.Property),
            InlineKeyboardButton.WithCallbackData("Персонал", CallbackData.Staff),
            //InlineKeyboardButton.WithCallbackData("Транспорт", CallbackData.Car),
            InlineKeyboardButton.WithCallbackData("Мечта", CallbackData.Dream),
            InlineKeyboardButton.WithCallbackData("Событие", CallbackData.Accident),
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back)
            );

        return new MessageToSend(ReplyText.ChooseCategoryToOpen, keyboard, false);
    }
    #endregion
}
