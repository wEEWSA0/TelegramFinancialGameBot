using Telegram.Bot.Types.ReplyMarkups;

using TelegramBotShit.Bot;
using TelegramBotShit.Bot.Buttons;
using TelegramBotShit.Router.Transmitted;

using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Service.Attributes;
using TelegramFinancialGameBot.Service.Router.Transmitted;
using TelegramFinancialGameBot.Util;
using TelegramFinancialGameBot.Util.Keyboard;
using TelegramFinancialGameBot.Util.Reply;

namespace TelegramFinancialGameBot.Service.Realization;

[BotMethodsClass]
public class StaffService
{
    private static InRoomStateReplyStorage ReplyText = ReplyStorage.InRoom;
    private static InRoomStateCallbackQueryStorage CallbackData = CallbackQueryStorage.InRoom;

    private UserRepository _userRepository;
    private BuisnessRepository _buisnessRepository; // TODO увольнение персонала стоит столько же энергии, сколько и найм
    private StaffRepository _staffRepository; // TODO проверить кнопки назад у staff (где-то была проблема)

    public StaffService()
    {
        _userRepository = new();
        _buisnessRepository = new();
        _staffRepository = new();
    }

    #region Menu
    public static MessageToSend GoToMenu(long chatId, TransmittedData data)
    {
        data.State = State.InRoom.StaffCategory.FullStaffInfo;
        //var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
        //    InlineKeyboardButton.WithCallbackData("Ассистента - 3 энергии", CallbackData.StaffCategory.StaffType),
        //    InlineKeyboardButton.WithCallbackData("Менеджера - 6 энергии", CallbackData.StaffCategory.ManagerType),
        //    InlineKeyboardButton.WithCallbackData("Регионального директора  - 12 энергии", CallbackData.StaffCategory.RegionalDirectorType),
        //    InlineKeyboardButton.WithCallbackData("Финансового директора  - 18 энергии", CallbackData.StaffCategory.FinancialDirectorType),
        //    InlineKeyboardButton.WithCallbackData("Генерального директора - 24 энергии", CallbackData.StaffCategory.GeneralDirecorType),
        //    InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back)
        //    );
        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Личный", CallbackData.StaffCategory.Own),
            InlineKeyboardButton.WithCallbackData("Бизнеса", CallbackData.StaffCategory.Biz),
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back)
            );

        if (data.Storage.TryGet<int>(ConstantsStorage.Page,out var page))
        {
            data.Storage.Delete(ConstantsStorage.Page); // todo попробовать страницы (вообще не тестировал)
        }

        return new MessageToSend("Ваш персонал:", keyboard, false);
    }


    [BotMethod(State.InRoom.StaffCategory.FullStaffInfo, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForMenu(TransmittedInfo info)
    {
        var userId = info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId);

        if (CallbackData.Back == info.Request)
        {
            return InRoomService.GoToCatalog(info.ChatId, info.Data);
            //var user = _userRepository.GetByIdOrThrow(userId);

            //return InRoomService.GoToMainMenu(info.ChatId, info.Data, user, user.Dream, user.Room);
        }

        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackData.StaffCategory.Own, GoToOwnStaffMenu),
            new (CallbackData.StaffCategory.Biz, GoToBizStaffMenu)
        });

    }

    #region Own
    private MessageToSend GoToOwnStaffMenu(long chatId, TransmittedData data)
    {
        var userId = data.Storage.GetOrThrow<int>(ConstantsStorage.UserId);

        var usersStaffList = _staffRepository.GetUserStaffList(userId);

        data.State = State.InRoom.StaffCategory.Own.ChooseStaff;

        InlineKeyboardMarkup? keyboard;

        if (usersStaffList.Count == 0)
        {
            keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

            return new MessageToSend("У вас нет личного персонала", keyboard, false);
        }

        var text = "Ваш персонал:";
        var staffOnPage = usersStaffList.Count;
        var count = staffOnPage + 1;
        int pageNum = 0;

        if (data.Storage.TryGet<int>(ConstantsStorage.Page, out var page))
        {
            pageNum = page;
        }

        if (usersStaffList.Count > 3)
        {
            staffOnPage = StaffConstParamters.StaffsPerPage;
            count = staffOnPage + 2;
        }

        var buttons = new List<List<InlineKeyboardButton>>();

        var staffCountOnPageWithPageOffset = (pageNum + 1) * staffOnPage;
        if (staffCountOnPageWithPageOffset > usersStaffList.Count)
        {
            staffOnPage = usersStaffList.Count;
        }

        for (int i = pageNum * staffOnPage; i < staffCountOnPageWithPageOffset; i++)
        {
            var staff = usersStaffList[i].Staff;

            var title = staff.Name;
            var callback = staff.Id;
            text += $"\n\n" +
                $"{i + 1}. {staff.Name}\n" +
                $"Фонд оплаты труда: {staff.CashExpense.ToString("n0")}₽\n" +
                $"Экономит времени: {staff.TimeIncome.ToString("n0")} часов";

            buttons.Add(new());
            buttons[i].Add(InlineKeyboardButton.WithCallbackData(title, callback));
        }

        var offset = staffOnPage;

        if (usersStaffList.Count > 3)
        {
            if (pageNum > 0)
            {
                var maxPages = 0;

                for (int i = 0; i < usersStaffList.Count; i += staffOnPage)
                {
                    maxPages++;
                }

                if (pageNum < maxPages)
                {
                    buttons[offset][0] = InlineKeyboardButton.WithCallbackData("Предыдущее", $"page:{pageNum--}");
                    buttons[offset][1] = InlineKeyboardButton.WithCallbackData("Следующее", $"page:{pageNum++}");
                }
                else
                {
                    buttons[offset][0] = InlineKeyboardButton.WithCallbackData("Предыдущее", $"page:{pageNum--}");
                }
            }
            else
            {
                buttons[offset][0] = InlineKeyboardButton.WithCallbackData("Следующее", $"page:{pageNum++}");
            }

            offset++;
        }

        buttons.Add(new());
        buttons[offset].Add(InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

        keyboard = new InlineKeyboardMarkup(buttons);

        return new MessageToSend(text, keyboard, false);
    }


    [BotMethod(State.InRoom.StaffCategory.Own.ChooseStaff, BotMethodType.Callback)]
    public MessageToSend ProcessChoosedOwnStaff(TransmittedInfo info)
    {
        if (info.Request == CallbackData.Back)
        {
            return GoToMenu(info.ChatId, info.Data);
        }

        var splitted = info.Request.Split(":");
        if (splitted[0] == "page")
        {
            var page = splitted[1];

            info.Data.Storage.Add(ConstantsStorage.Page, page);

            return ProcessButtonsForMenu(info);
        }

        var userId = info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId);
        if (!_staffRepository.TryGetUserStaff(userId, info.Request, out var staff))
        {
            throw new NotSupportedException("genDir " + info.Request);
        }

        info.Data.Storage.Add(ConstantsStorage.Staff, staff);

        InlineKeyboardMarkup keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Уволить", CallbackData.StaffCategory.Dismiss),
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back)
            );

        info.Data.State = State.InRoom.StaffCategory.Own.ChooseAction;

        return new MessageToSend("Что хотите сделать", keyboard, false);
    }


    [BotMethod(State.InRoom.StaffCategory.Own.ChooseAction, BotMethodType.Callback)]
    public MessageToSend ProcessActionWithOwnStaff(TransmittedInfo info)
    {
        if (info.Request == CallbackData.Back)
        {
            return GoToOwnStaffMenu(info.ChatId, info.Data);
        }

        var staff = info.Data.Storage.GetOrThrow<UserStaff>(ConstantsStorage.Staff);
        var userId = info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId);
        var user = _userRepository.GetByIdOrThrow(userId);

        if (user.FreeTime < staff.Staff.TimeIncome)
        {
            RealizationUtil.AddMessageToStack(info.ChatId, new MessageToSend("Вам не хватает времени", false));

            return GoToMenu(info.ChatId, info.Data).With(true);
        }

        user.CashIncome += staff.Staff.CashExpense;
        user.FreeTime -= staff.Staff.TimeIncome;

        _userRepository.Update(user);

        _staffRepository.Remove(staff);

        RealizationUtil.AddMessageToStack(info.ChatId, new MessageToSend("Сотрудник уволен", false));

        return GoToMenu(info.ChatId, info.Data).With(true);
    }
    #endregion
    #region Biz
    private MessageToSend GoToBizStaffMenu(long chatId, TransmittedData data)
    {
        var userId = data.Storage.GetOrThrow<int>(ConstantsStorage.UserId);

        data.State = State.InRoom.StaffCategory.Biz.ChooseBuisness;

        var buisnesses = _buisnessRepository.GetUserBuisnessList(userId);

        var text = ReplyText.YourBuisnesses;

        if (buisnesses.Count == 0)
        {
            text = "У вас нет подходящего бизнеса";
        }

        return new MessageToSend(text, BuisnessService.GetBuisnessesKeyboard(buisnesses.ToArray()), false);
    }


    [BotMethod(State.InRoom.StaffCategory.Biz.ChooseBuisness, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForBizChooseBuisness(TransmittedInfo info)
    {
        if (CallbackData.Back == info.Request)
        {
            return GoToMenu(info.ChatId, info.Data);
        }

        if (!int.TryParse(info.Request, out var bizId))
        {
            throw new NotSupportedException(info.Request);
        }

        if (!_buisnessRepository.TryGetUserBuisness(bizId, out var buisness))
        {
            throw new NotSupportedException(info.Request);
        }

        info.Data.Storage.Add(ConstantsStorage.Buisness, buisness);

        info.Data.State = State.InRoom.StaffCategory.Biz.ChooseStaffType;

        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Менеджеры", CallbackData.StaffCategory.ManagerType),
            InlineKeyboardButton.WithCallbackData("Региональные директора", CallbackData.StaffCategory.RegionalDirectorType),
            InlineKeyboardButton.WithCallbackData("Ген. и фин. директора", CallbackData.StaffCategory.FinAndGenDirectorTypes),
                    InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

        return new MessageToSend("Выберите должность сотрудников", keyboard, false);
    }


    [BotMethod(State.InRoom.StaffCategory.Biz.ChooseStaffType, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForBizChooseStaffType(TransmittedInfo info)
    {
        if (CallbackData.Back == info.Request)
        {
            return GoToBizStaffMenu(info.ChatId, info.Data);
        }

        var userId = info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId);

        info.Data.State = State.InRoom.StaffCategory.Biz.ChooseStaff;

        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackData.StaffCategory.ManagerType, GoToManagerMenu),
            new (CallbackData.StaffCategory.RegionalDirectorType, GoToRegionalDirectorMenu),
            new (CallbackData.StaffCategory.FinAndGenDirectorTypes, GoToGenAndFinDirectorMenu)
        });

        // todo можно вынести огромный кусок кода, который повторяетя раза 4. Однако он с разными типами
        MessageToSend GoToManagerMenu(long chatId, TransmittedData data)
        {
            var usersStaffList = _staffRepository.GetBuisnessManagerStaffList(userId);

            InlineKeyboardMarkup? keyboard;

            if (usersStaffList.Count == 0)
            {
                keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

                return new MessageToSend("У вас нет персонала", keyboard, false);
            }

            var text = "Ваш персонал:";
            var staffOnPage = usersStaffList.Count;
            var count = staffOnPage + 1;
            int pageNum = 0;

            if (data.Storage.TryGet<int>(ConstantsStorage.Page, out var page))
            {
                pageNum = page;
            }

            if (usersStaffList.Count > 3)
            {
                staffOnPage = StaffConstParamters.StaffsPerPage;
                count = staffOnPage + 2;
            }

            var buttons = new List<List<InlineKeyboardButton>>();

            var staffCountOnPageWithPageOffset = (pageNum + 1) * staffOnPage;
            if (staffCountOnPageWithPageOffset > usersStaffList.Count)
            {
                staffOnPage = usersStaffList.Count;
            }

            for (int i = pageNum * staffOnPage; i < staffCountOnPageWithPageOffset; i++)
            {
                var staff = usersStaffList[i].ManagerStaff;

                var title = staff.Name;
                var callback = staff.Id;
                text += $"\n\n" +
                    $"{i + 1}. {staff.Name}\n" +
                    $"Фонд оплаты труда: {staff.CashExpense.ToString("n0")}₽\n" +
                    $"Экономит времени: {staff.TimeIncome.ToString("n0")} часов";

                buttons.Add(new List<InlineKeyboardButton>());
                buttons[i - (pageNum * staffOnPage)].Add(InlineKeyboardButton.WithCallbackData(title, callback));
            }

            var offset = staffOnPage;

            if (usersStaffList.Count > 3)
            {
                if (pageNum > 0)
                {
                    var maxPages = 0;

                    for (int i = 0; i < usersStaffList.Count; i += staffOnPage)
                    {
                        maxPages++;
                    }

                    if (pageNum < maxPages)
                    {
                        buttons[offset][0] = InlineKeyboardButton.WithCallbackData("Предыдущее", $"page:{pageNum--}");
                        buttons[offset][1] = InlineKeyboardButton.WithCallbackData("Следующее", $"page:{pageNum++}");
                    }
                    else
                    {
                        buttons[offset][0] = InlineKeyboardButton.WithCallbackData("Предыдущее", $"page:{pageNum--}");
                    }
                }
                else
                {
                    buttons[offset][0] = InlineKeyboardButton.WithCallbackData("Следующее", $"page:{pageNum++}");
                }

                offset++;
            }

            buttons.Add(new List<InlineKeyboardButton>());
            buttons[offset].Add(InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

            keyboard = new InlineKeyboardMarkup(buttons);

            info.Data.State = State.InRoom.StaffCategory.Biz.ChooseStaff;

            return new MessageToSend(text, keyboard, false);

            //var usersStaffList = _staffRepository.GetBuisnessManagerStaffList(userId);

            //InlineKeyboardMarkup? keyboard;

            //if (usersStaffList.Count == 0)
            //{
            //    keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            //        InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

            //    return new MessageToSend("У вас нет персонала", keyboard, false);
            //}

            //var text = "Ваш персонал:";
            //var staffOnPage = usersStaffList.Count;
            //var count = staffOnPage + 1;
            //int pageNum = 0;

            //if (info.Data.Storage.TryGet<int>(ConstantsStorage.Page, out var page))
            //{
            //    pageNum = page;
            //}

            //if (usersStaffList.Count > 3)
            //{
            //    staffOnPage = StaffConstParamters.StaffsPerPage;
            //    count = staffOnPage + 2;
            //}

            //var buttons = new InlineKeyboardButton[count][];

            //var n = (pageNum + 1) * staffOnPage;
            //if (n > usersStaffList.Count)
            //{
            //    n = usersStaffList.Count;
            //}

            //for (int i = pageNum * staffOnPage; i < n; i++)
            //{
            //    var staff = usersStaffList[i].ManagerStaff;

            //    var title = staff.Name;
            //    var callback = staff.Id;
            //    text += $"\n\n" +
            //        $"{i}. {staff.Name}\n" +
            //        $"Фонд оплаты труда: {staff.CashExpense.ToString("n0")}₽\n" +
            //        $"Экономит времени: {staff.TimeIncome.ToString("n0")} часов";

            //    buttons[i][0] = InlineKeyboardButton.WithCallbackData(title, callback);
            //}

            //if (usersStaffList.Count > 3)
            //{
            //    if (pageNum > 0)
            //    {
            //        var maxPages = 0;

            //        for (int i = 0; i < usersStaffList.Count; i += staffOnPage)
            //        {
            //            maxPages++;
            //        }

            //        if (pageNum < maxPages)
            //        {
            //            buttons[staffOnPage][0] = InlineKeyboardButton.WithCallbackData("Предыдущее", $"page:{pageNum--}");
            //            buttons[staffOnPage][1] = InlineKeyboardButton.WithCallbackData("Следующее", $"page:{pageNum++}");
            //        }
            //        else
            //        {
            //            buttons[staffOnPage][0] = InlineKeyboardButton.WithCallbackData("Предыдущее", $"page:{pageNum--}");
            //        }
            //    }
            //    else
            //    {
            //        buttons[staffOnPage][0] = InlineKeyboardButton.WithCallbackData("Следующее", $"page:{pageNum++}");
            //    }
            //}

            //buttons[staffOnPage + 1][0] = InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back);

            //keyboard = new InlineKeyboardMarkup(buttons);

            //return new MessageToSend(text, keyboard, false);
        }

        MessageToSend GoToRegionalDirectorMenu(long chatId, TransmittedData data)
        {
            var usersStaffList = _staffRepository.GetBuisnessRegionalDirectorList(userId);

            InlineKeyboardMarkup? keyboard;

            if (usersStaffList.Count == 0)
            {
                keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

                return new MessageToSend("У вас нет персонала", keyboard, false);
            }

            var text = "Ваш персонал:";
            var staffOnPage = usersStaffList.Count;
            var count = staffOnPage + 1;
            int pageNum = 0;

            if (info.Data.Storage.TryGet<int>(ConstantsStorage.Page, out var page))
            {
                pageNum = page;
            }

            if (usersStaffList.Count > 3)
            {
                staffOnPage = StaffConstParamters.StaffsPerPage;
                count = staffOnPage + 2;
            }

            var buttons = new InlineKeyboardButton[count][];

            var n = (pageNum + 1) * staffOnPage;
            if (n > usersStaffList.Count)
            {
                n = usersStaffList.Count;
            }

            for (int i = pageNum * staffOnPage; i < n; i++)
            {
                var staff = usersStaffList[i].RegionalDirector;

                var title = staff.Name;
                var callback = staff.Id;
                text += $"\n\n" +
                    $"{i}. {staff.Name}\n" +
                    $"Фонд оплаты труда: {staff.CashExpense.ToString("n0")}₽\n" +
                    $"Экономит времени: {staff.TimeIncome.ToString("n0")} часов";

                buttons[i][0] = InlineKeyboardButton.WithCallbackData(title, callback);
            }

            if (usersStaffList.Count > 3)
            {
                if (pageNum > 0)
                {
                    var maxPages = 0;

                    for (int i = 0; i < usersStaffList.Count; i += staffOnPage)
                    {
                        maxPages++;
                    }

                    if (pageNum < maxPages)
                    {
                        buttons[staffOnPage][0] = InlineKeyboardButton.WithCallbackData("Предыдущее", $"page:{pageNum--}");
                        buttons[staffOnPage][1] = InlineKeyboardButton.WithCallbackData("Следующее", $"page:{pageNum++}");
                    }
                    else
                    {
                        buttons[staffOnPage][0] = InlineKeyboardButton.WithCallbackData("Предыдущее", $"page:{pageNum--}");
                    }
                }
                else
                {
                    buttons[staffOnPage][0] = InlineKeyboardButton.WithCallbackData("Следующее", $"page:{pageNum++}");
                }
            }

            buttons[staffOnPage + 1][0] = InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back);

            keyboard = new InlineKeyboardMarkup(buttons);

            info.Data.State = State.InRoom.StaffCategory.Biz.ChooseStaff;

            return new MessageToSend(text, keyboard, false);
        }

        MessageToSend GoToGenAndFinDirectorMenu(long chatId, TransmittedData data)
        {
            var buisness = data.Storage.GetOrThrow<UserBuisness>(ConstantsStorage.Buisness);

            InlineKeyboardMarkup? keyboard;

            if (buisness.FinancialDirectorId is null && buisness.GeneralDirectorId is null)
            {
                keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

                return new MessageToSend("У вас нет персонала", keyboard, false);
            }

            var text = "Ваш персонал:";
            var staffOnPage = 2;

            List<InlineKeyboardButton> buttons = new();

            var genDir = buisness.GeneralDirector;

            var title = genDir.Name;
            var callback = genDir.Id;
            text += $"\n\n" +
                $"1. {genDir.Name}\n" +
                $"Фонд оплаты труда: {genDir.CashExpense.ToString("n0")}₽\n" +
                $"Экономит времени: {genDir.TimeIncome.ToString("n0")} часов\n" +
                $"Прибыль бизнеса: {genDir.CashIncomePercent.ToString("n0")}%";

            var finDir = buisness.FinancialDirector;

            title = finDir.Name;
            callback = finDir.Id;
            text += $"\n\n" +
                $"2. {finDir.Name}\n" +
                $"Фонд оплаты труда: {finDir.CashExpense.ToString("n0")}₽\n" +
                $"Экономит времени: {finDir.TimeIncome.ToString("n0")} часов\n" +
                $"Прибыль бизнеса: {finDir.CashIncomePercent.ToString("n0")}%";

            buttons.Add(InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

            keyboard = new InlineKeyboardMarkup(buttons);

            return new MessageToSend(text, keyboard, false);
        }
    }


    [BotMethod(State.InRoom.StaffCategory.Biz.ChooseStaff, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForBizChooseStaff(TransmittedInfo info)
    {
        if (CallbackData.Back == info.Request)
        {
            var buisness = info.Data.Storage.GetOrThrow<UserBuisness>(ConstantsStorage.Buisness);

            var request = buisness.Id.ToString();

            return ProcessButtonsForBizChooseBuisness(info.GetNewWithRequest(request));
        }

        info.Data.State = State.InRoom.StaffCategory.Biz.ChooseAction;

        info.Data.Storage.Add(ConstantsStorage.StaffId, info.Request);

        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Уволить", CallbackData.StaffCategory.Dismiss),
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

        return new MessageToSend("Что хотите сделать", keyboard, false);
    }


    [BotMethod(State.InRoom.StaffCategory.Biz.ChooseAction, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForActionWithBizStaff(TransmittedInfo info)
    {
        var staffId = info.Data.Storage.GetOrThrow<string>(ConstantsStorage.StaffId);

        if (CallbackData.Back == info.Request)
        {
            return ProcessButtonsForBizChooseStaffType(info.GetNewWithRequest(staffId));
        }

        if (CallbackData.StaffCategory.Dismiss != info.Request)
        {
            throw new NotImplementedException(info.Request);
        }

        var buisness = info.Data.Storage.GetOrThrow<UserBuisness>(ConstantsStorage.Buisness);

        var userId = info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId);
        var user = _userRepository.GetByIdOrThrow(userId);

        MessageToSend? messageToSend = null;

        if (buisness.FinancialDirectorId == info.Request)
        {
            var dir = buisness.FinancialDirector;

            messageToSend = Process(dir.TimeIncome, dir.CashExpense);

            buisness.FinancialDirector = null;
            buisness.FinancialDirectorId = null;

            _buisnessRepository.Update(buisness);
        }

        if (buisness.GeneralDirectorId == info.Request)
        {
            var dir = buisness.GeneralDirector;

            messageToSend = Process(dir.TimeIncome, dir.CashExpense);

            buisness.GeneralDirector = null;
            buisness.GeneralDirectorId = null;

            _buisnessRepository.Update(buisness);
        }

        if (_staffRepository.TryGetBuisnessManagerStaff(buisness.Id, info.Request, out var manager))
        {
            var staff = manager.ManagerStaff;

            messageToSend = Process(staff.TimeIncome, staff.CashExpense);
        }

        if (_staffRepository.TryGetBuisnessRegionalDirector(buisness.Id, info.Request, out var regDirector))
        {
            var staff = regDirector.RegionalDirector;

            messageToSend = Process(staff.TimeIncome, staff.CashExpense);
        }

        if (messageToSend is null)
        {
            throw new NotSupportedException(info.Request);
        }

        _userRepository.Update(user);

        return messageToSend;

        MessageToSend Process(short timeIncome, int cashExpense)
        {
            if (user.FreeTime < timeIncome)
            {
                RealizationUtil.AddMessageToStack(info.ChatId, new MessageToSend("Вам не хватает времени", false));

                return GoToMenu(info.ChatId, info.Data).With(true);
            }

            user.CashIncome += cashExpense;
            user.FreeTime -= timeIncome;

            RealizationUtil.AddMessageToStack(info.ChatId, new MessageToSend("Сотрудник уволен", false));

            return GoToMenu(info.ChatId, info.Data).With(true);
        }
    }
    #endregion


    #endregion
    #region Add

    public MessageToSend GoToAddMenu(long chatId, TransmittedData data, ManagerStaff staff)
    {
        return GoToAddMenu<ManagerStaff>(chatId, data, staff, StaffConstParamters.EnergyToManager);
    }

    public MessageToSend GoToAddMenu(long chatId, TransmittedData data, RegionalDirector staff)
    {
        return GoToAddMenu<RegionalDirector>(chatId, data, staff, StaffConstParamters.EnergyToRegionalDirector);
    }

    public MessageToSend GoToAddMenu(long chatId, TransmittedData data, GeneralDirector staff)
    {
        return GoToAddMenu<GeneralDirector>(chatId, data, staff, StaffConstParamters.EnergyToGeneralDirector);
    }

    public MessageToSend GoToAddMenu(long chatId, TransmittedData data, FinancialDirector staff)
    {
        return GoToAddMenu<FinancialDirector>(chatId, data, staff, StaffConstParamters.EnergyToFinancialDirector);
    }

    public MessageToSend GoToAddMenu(long chatId, TransmittedData data, Staff staff)
    {
        return GoToAddMenu<Staff>(chatId, data, staff, StaffConstParamters.EnergyToStaff);
    }

    private MessageToSend GoToAddMenu<T>(long chatId, TransmittedData data, T staff, short energy)
    {
        var user = _userRepository.GetByIdOrThrow(data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

        if (user.Energy < energy)
        {
            return new MessageToSend($"Вам не хватает энергии. Необходимо еще {energy - user.Energy} энергии");
        }

        data.Storage.Add(ConstantsStorage.Staff, staff);

        InlineKeyboardMarkup? keyboard;

        if (typeof(T) == typeof(Staff))
        {
            keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Да", CallbackData.Yes),
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

            data.State = State.InRoom.StaffCategory.Own.Add.ChooseAction;

            return new MessageToSend("Вы хотите нанять?", keyboard);
        }
        else
        {
            var buisnesses = _buisnessRepository.GetUserBuisnessList(data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

            data.State = State.InRoom.StaffCategory.Biz.Add.ChooseBuisness;

            if (buisnesses.Count == 0)
            {
                keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

                return new MessageToSend("У вас нет подходящего бизнеса", keyboard);
            }

            keyboard = BuisnessService.GetBuisnessesKeyboard(buisnesses.ToArray());

            return new MessageToSend("В какой бизнес хотите нанять?", keyboard, false);
        }
    }


    [BotMethod(State.InRoom.StaffCategory.Own.Add.ChooseAction, BotMethodType.Callback)]
    public MessageToSend ProcessAddOwn(TransmittedInfo info)
    {
        var staff = info.Data.Storage.GetOrThrow<Staff>(ConstantsStorage.Staff);

        if (info.Request == CallbackData.Back)
        {
            //return GoToAddMenu(info.ChatId, info.Data, staff).With(false);
            //return GoToMenu(info.ChatId, info.Data);
            return InRoomService.GoToOpenCard(info.ChatId, info.Data);
        }

        var user = _userRepository.GetByIdOrThrow(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

        //var difIncome = user.CashIncome - staff.CashExpense;

        //if (difIncome < 0)
        //{
        //    var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
        //InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

        //    return new MessageToSend(ReplyText.PurchaseFail(null, null, difIncome, null), keyboard, false);
        //}

        user.FreeTime += staff.TimeIncome; // TODO после еды проверить менеджеров - персонала бизнеса (добавление и меню)
        user.CashIncome -= staff.CashExpense; // TODO после проверки менеджеров или в процессе убрать проверку на cashIncome

        _staffRepository.AddNew(new UserStaff
        {
            UserId = user.Id,
            StaffId = staff.Id
        });

        _userRepository.Update(user);

        //return GoToMenu(info.ChatId, info.Data);
        return InRoomService.GoToMainMenu(info.ChatId, info.Data, user, user.Dream, user.Room); ;
    }


    [BotMethod(State.InRoom.StaffCategory.Biz.Add.ChooseBuisness, BotMethodType.Callback)]
    public MessageToSend ProcessAddBizChooseBuisness(TransmittedInfo info)
    {
        if (info.Request == CallbackData.Back)
        {
            //return GoToMenu(info.ChatId, info.Data);
            return InRoomService.GoToOpenCard(info.ChatId, info.Data);
        }

        if (!int.TryParse(info.Request, out var buisnessId))
        {
            throw new DirectoryNotFoundException(info.Request);
        }

        if (!_buisnessRepository.TryGetUserBuisness(buisnessId, out var buisness))
        {
            throw new DirectoryNotFoundException(info.Request);
        }

        info.Data.Storage.Add(ConstantsStorage.Buisness, buisness);

        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Да", CallbackData.Yes),
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

        info.Data.State = State.InRoom.StaffCategory.Biz.Add.ChooseAction;

        return new MessageToSend("Вы хотите нанять?", keyboard, false);
    }


    [BotMethod(State.InRoom.StaffCategory.Biz.Add.ChooseAction, BotMethodType.Callback)]
    public MessageToSend ProcessAddBiz(TransmittedInfo info)
    {
        var buisness = info.Data.Storage.GetOrThrow<UserBuisness>(ConstantsStorage.Buisness);

        var staffObj = info.Data.Storage.GetOrThrow(ConstantsStorage.Staff);

        if (info.Request == CallbackData.Back) // todo здесь есть какой-то баг с GetOrThrow
        {
            if (staffObj is ManagerStaff)
            {
                return GoToAddMenu(info.ChatId, info.Data, (ManagerStaff)staffObj);
            }
            else if (staffObj is RegionalDirector)
            {
                return GoToAddMenu(info.ChatId, info.Data, (RegionalDirector)staffObj);
            } 
            else if (staffObj is FinancialDirector)
            {
                return GoToAddMenu(info.ChatId, info.Data, (FinancialDirector)staffObj);
            } 
            else if (staffObj is GeneralDirector)
            {
                return GoToAddMenu(info.ChatId, info.Data, (GeneralDirector)staffObj);
            }
            //return ProcessAddBizChooseBuisness(info.GetNewWithRequest(buisness.Id.ToString()));
            //return GoToMenu(info.ChatId, info.Data);
            ///TODO ПРЯМО СЕЙЧАС
            ///доделать кнопки назад у персонала, потом перевести все кнопки в каталог
            ///проверить списывание энергии
            ///Списывать энергию за увольнение
        }

        var user = _userRepository.GetByIdOrThrow(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));
        
        bool isCompleted = true; // todo придумать способ для хранения таких объектов
        switch (staffObj)
        {
            case ManagerStaff:
                {
                    var staff = (ManagerStaff)staffObj;
                    var difIncome = user.CashIncome - staff.CashExpense;

                    var staffs = _staffRepository.GetBuisnessManagerStaffList(buisness.Id);

                    if (difIncome < 0 || buisness.BranchCount < staffs.Count * StaffConstParamters.ManagersPerBranches)
                    {
                        isCompleted = false;
                    }
                    else
                    {
                        user.FreeTime += staff.TimeIncome;
                        user.CashIncome -= staff.CashExpense;

                        _staffRepository.AddNew(new BuisnessManagerStaff
                        {
                            UserBuisnessId = buisness.Id,
                            ManagerStaffId = staff.Id
                        });
                    }
                } break;
            case RegionalDirector:
                {
                    var staff = (RegionalDirector)staffObj;
                    var difIncome = user.CashIncome - staff.CashExpense;

                    var staffs = _staffRepository.GetBuisnessRegionalDirectorList(buisness.Id);

                    if (difIncome < 0 || buisness.BranchCount < staffs.Count * StaffConstParamters.RegionalDirectorsPerBranches)
                    {
                        isCompleted = false;
                    }
                    else
                    {
                        user.FreeTime += staff.TimeIncome;
                        user.CashIncome -= staff.CashExpense;

                        _staffRepository.AddNew(new BuisnessRegionalDirector
                        {
                            UserBuisnessId = buisness.Id,
                            RegionalDirectorId = staff.Id
                        });
                    }
                }
                break;
            case FinancialDirector:
                {
                    var staff = (FinancialDirector)staffObj;
                    var difIncome = user.CashIncome - staff.CashExpense;

                    if (difIncome < 0 || buisness.FinancialDirectorId != null)
                    {
                        isCompleted = false;
                    }
                    else
                    {
                        user.FreeTime += staff.TimeIncome;
                        user.CashIncome -= staff.CashExpense;

                        _staffRepository.AddNew(staff, buisness.Id);
                    }
                }
                break;
            case GeneralDirector:
                {
                    var staff = (GeneralDirector)staffObj;
                    var difIncome = user.CashIncome - staff.CashExpense;

                    if (difIncome < 0 || buisness.GeneralDirectorId != null)
                    {
                        isCompleted = false;
                    }
                    else
                    {
                        user.FreeTime += staff.TimeIncome;
                        user.CashIncome -= staff.CashExpense;

                        _staffRepository.AddNew(staff, buisness.Id);
                    }
                }
                break;
        }

        if (!isCompleted)
        {
            var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
        InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

            return new MessageToSend("Ошибка покупки", keyboard, false);
        }

        _userRepository.Update(user);

        return GoToMenu(info.ChatId, info.Data);
    }

    #endregion
    #region Service
    //public void ForceAddBuisness(long chatId, TransmittedData data, Buisness genDir)
    //{
    //    var userId = data.Storage.GetOrThrow<int>(ConstantsStorage.UserId);
    //    var user = _userRepository.GetByIdOrThrow(userId);

    //    _buisnessRepository.AddNew(new UserBuisness
    //    {
    //        UserId = user.Id,
    //        BranchCount = 1,
    //        BuisnessId = genDir.Id,
    //        OpenSteps = 0
    //    });

    //    user.FreeTime -= genDir.RequireTime;
    //    user.CashIncome -= genDir.CashExpense;

    //    _userRepository.Update(user);
    //}

    //public List<UserBuisness> GetUserBuisnessList(int userId)
    //{
    //    return _buisnessRepository.GetUserBuisnessList(userId);
    //}

    //public void UpdateUserBuisnessList(List<UserBuisness> userBuisnesses)
    //{
    //    _buisnessRepository.UpdateRange(userBuisnesses);
    //}

    public bool TryGetStaff(string id, out Staff staff)
    {
        return _staffRepository.TryGetStaff(id, out staff);
    }

    public List<Staff> GetStaffList(int userId)
    {
        return _staffRepository.GetStaffList(userId);
    }

    public bool TryGetManagerStaff(string id, out ManagerStaff staff)
    {
        return _staffRepository.TryGetManagerStaff(id, out staff);
    }

    public List<ManagerStaff> GetManagerStaffList(int userId)
    {
        return _staffRepository.GetUserManagerStaffList(userId);
    }

    public bool TryGetRegionalDirector(string id, out RegionalDirector staff)
    {
        return _staffRepository.TryGetRegionalDirector(id, out staff);
    }

    public List<RegionalDirector> GetRegionalDirectorList(int userId)
    {
        return _staffRepository.GetUserRegionalDirectorList(userId);
    }

    public bool TryGetFinancialDirector(string id, out FinancialDirector staff)
    {
        return _staffRepository.TryGetFinancialDirector(id, out staff);
    }

    public List<FinancialDirector> GetFinancialDirectorList(int userId)
    {
        return _staffRepository.GetUserFinancialDirectorList(userId);
    }

    public bool TryGetGeneralDirector(string id, out GeneralDirector staff)
    {
        return _staffRepository.TryGetGeneralDirector(id, out staff);
    }

    public List<GeneralDirector> GetGeneralDirectorList(int userId)
    {
        return _staffRepository.GetUserGeneralDirectorList(userId);
    }

    public void ForceAddStaff(Model.User user, Staff staff)
    {
        var personal = _staffRepository.AddNew(new UserStaff
        {
            UserId = user.Id,
            StaffId = staff.Id
        });

        user.FreeTime += personal.Staff.TimeIncome;
        user.CashIncome -= personal.Staff.CashExpense;
    }

    #endregion
}
