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
public class BuisnessService
{
    private static InRoomStateReplyStorage ReplyText = ReplyStorage.InRoom;
    private static InRoomStateCallbackQueryStorage CallbackData = CallbackQueryStorage.InRoom;

    private UserRepository _userRepository;
    private BuisnessRepository _buisnessRepository;

    public BuisnessService()
    {
        _userRepository = new UserRepository();
        _buisnessRepository = new BuisnessRepository();
    }

    #region Menu
    public MessageToSend GoToMenu(long chatId, TransmittedData data)
    {
        var userId = data.Storage.GetOrThrow<int>(ConstantsStorage.UserId);

        var userBuisnessList = _buisnessRepository.GetUserBuisnessList(userId);

        data.State = State.InRoom.BuisnessCategory.Info;

        InlineKeyboardMarkup? keyboard;

        if (userBuisnessList.Count == 0)
        {
            keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

            return new MessageToSend("У вас нет бизнеса", keyboard, false);
        }

        keyboard = GetBuisnessesKeyboard(userBuisnessList.ToArray());

        return new MessageToSend(ReplyText.YourBuisnesses, keyboard, false);
    }

    public static InlineKeyboardMarkup GetBuisnessesKeyboard(UserBuisness[] buisnesses)
    {
        List<InlineKeyboardButton> buttons = new();

        foreach (UserBuisness buisness in buisnesses)
        {
            buttons.Add(
                InlineKeyboardButton.WithCallbackData($"{buisness.Buisness.Name} ({buisness.BranchCount} филиалы)", buisness.Id.ToString())
                );
        }

        buttons.Add(InlineKeyboardButton.WithCallbackData("Назад", CallbackQueryStorage.InRoom.Back));

        return BotKeyboardCreator.Instance.GetKeyboardMarkup(buttons.ToArray());
    }


    [BotMethod(State.InRoom.BuisnessCategory.Info, BotMethodType.Callback)]
    public MessageToSend ProcessChoosedBuisness(TransmittedInfo info)
    {
        if (info.Request == CallbackData.Back)
        {
            var user = _userRepository.GetByIdOrThrow(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

            return InRoomService.GoToMainMenu(info.ChatId, info.Data, user, user.Dream, user.Room).With(false);
        }

        if (!_buisnessRepository.TryGetUserBuisness(int.Parse(info.Request), out var buisness))
        {
            throw new KeyNotFoundException();
        }

        InlineKeyboardMarkup keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Открыть филиал (12 энергии)", CallbackData.BuisnessCategory.OpenBranch),
            InlineKeyboardButton.WithCallbackData("Персонал", CallbackData.BuisnessCategory.Staff),
            InlineKeyboardButton.WithCallbackData("Продать", CallbackData.BuisnessCategory.Sale),
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back)
            );

        info.Data.State = State.InRoom.BuisnessCategory.ChooseAction;

        info.Data.Storage.Add(ConstantsStorage.Buisness, buisness);

        return new MessageToSend(ReplyText.BuisnessInfo(buisness), keyboard, false);
    }


    [BotMethod(State.InRoom.BuisnessCategory.ChooseAction, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForActionWithBuisness(TransmittedInfo info)
    {
        var buisness = info.Data.Storage.GetOrThrow<UserBuisness>(ConstantsStorage.Buisness);

        var user = _userRepository.GetByIdOrThrow(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackData.BuisnessCategory.Sale, ProcessButtonSale),
            new (CallbackData.BuisnessCategory.OpenBranch, ProcessButtonOpenBranch),
            new (CallbackData.BuisnessCategory.Staff, ProcessButtonStaff),
            new (CallbackData.Back, ProcessButtonBack)
        });

        MessageToSend ProcessButtonSale(long chatId, TransmittedData data)
        {
            data.State = State.InRoom.BuisnessCategory.Sale;

            var energy = BuisnessConstParamters.EnergyToSale;

            var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData($"Продать весь бизнес - {energy} энергии", CallbackData.BuisnessCategory.SaleBuisness),
            InlineKeyboardButton.WithCallbackData($"Продать 1 филиал - {energy} энергии", CallbackData.BuisnessCategory.SaleOneBranch),
            InlineKeyboardButton.WithCallbackData($"Указать кол-во филиалов - {energy} энергии", CallbackData.BuisnessCategory.SaleAnyBranches),
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back)
            );

            return new MessageToSend(ReplyText.BuisnessesSaleType, keyboard, false);
        }

        MessageToSend ProcessButtonOpenBranch(long chatId, TransmittedData data)
        {
            var difCash = user.Cash - buisness.Buisness.Cost;
            var difTime = user.FreeTime - buisness.Buisness.RequireTime;
            var difEnergy = user.Energy - BuisnessConstParamters.EnergyToOpenBrench;

            if (difCash < 0 || difTime < 0 || difEnergy < 0)
            {
                var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

                return new MessageToSend(ReplyText.PurchaseFail(difCash, difTime, null, difEnergy), keyboard, false);
            }

            user.Cash -= buisness.Buisness.Cost;
            user.FreeTime -= buisness.Buisness.RequireTime;
            user.Energy -= BuisnessConstParamters.EnergyToOpenBrench;
            user.CashIncome += buisness.Buisness.CashIncome - buisness.Buisness.CashExpense;

            buisness.BranchCount++;

            _buisnessRepository.Update(buisness);

            _userRepository.Update(user);

            return GoToMenu(chatId, data);
        }

        MessageToSend ProcessButtonStaff(long chatId, TransmittedData data)
        {
            return StaffService.GoToMenu(chatId, data);
        }

        MessageToSend ProcessButtonBack(long chatId, TransmittedData data)
        {
            return GoToMenu(chatId, data);
        }
    }


    [BotMethod(State.InRoom.BuisnessCategory.Sale, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForSale(TransmittedInfo info)
    {
        var buisness = info.Data.Storage.GetOrThrow<UserBuisness>(ConstantsStorage.Buisness);

        var user = _userRepository.GetByIdOrThrow(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

        if (info.Request != CallbackData.Back)
        {
            if (user.Energy - BuisnessConstParamters.EnergyToSale < 0)
            {
                RealizationUtil.AddMessageToStack(info.ChatId, new MessageToSend("Не хватает энергии", false));

                return GoToMenu(info.ChatId, info.Data).With(true);
            }

            user.Energy -= BuisnessConstParamters.EnergyToSale;
            _userRepository.Update(user);
        }

        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackData.BuisnessCategory.SaleBuisness, ProcessButtonSaleBuisness),
            new (CallbackData.BuisnessCategory.SaleOneBranch, ProcessButtonSaleOneBranch),
            new (CallbackData.BuisnessCategory.SaleAnyBranches, ProcessButtonSaleAnyBranches),
            new (CallbackData.Back, ProcessButtonBack)
        });

        MessageToSend ProcessButtonSaleBuisness(long chatId, TransmittedData data)
        {
            data.State = State.InRoom.BuisnessCategory.SaleBuisness;

            var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Да", CallbackData.Yes),
            InlineKeyboardButton.WithCallbackData("Нет", CallbackData.No));

            return new MessageToSend(ReplyText.BuisnessSale(buisness, 100, buisness.Buisness.Cost), keyboard, false);
        }

        MessageToSend ProcessButtonSaleOneBranch(long chatId, TransmittedData data)
        {
            data.State = State.InRoom.BuisnessCategory.SaleOne;

            var fakeBuisness = new UserBuisness
            {
                BranchCount = 1,
                Buisness = buisness.Buisness
            };

            var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Да", CallbackData.Yes),
            InlineKeyboardButton.WithCallbackData("Нет", CallbackData.No));

            return new MessageToSend(ReplyText.BuisnessSale(fakeBuisness, 100, buisness.Buisness.Cost), keyboard, false);
        }

        MessageToSend ProcessButtonSaleAnyBranches(long chatId, TransmittedData data)
        {
            data.State = State.InRoom.BuisnessCategory.InputCountForSaleAny;

            return new MessageToSend("Введите количество филиалов для продажи", false);
        }

        MessageToSend ProcessButtonBack(long chatId, TransmittedData data)
        {
            return GoToMenu(chatId, data);
        }
    }


    [BotMethod(State.InRoom.BuisnessCategory.SaleBuisness, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForSaleBuisness(TransmittedInfo info)
    {
        var buisness = info.Data.Storage.GetOrThrow<UserBuisness>(ConstantsStorage.Buisness);

        var user = _userRepository.GetByIdOrThrow(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackData.Yes, ProcessButtonYes),
            new (CallbackData.No, ProcessButtonNo),
            new (CallbackData.Back, ProcessButtonNo)
        });


        MessageToSend ProcessButtonYes(long chatId, TransmittedData data)
        {
            var b = buisness.Buisness;

            user.Cash += b.Cost * buisness.BranchCount;

            if (buisness.OpenSteps >= 0)
            {
                user.CashIncome -= (b.CashIncome - (int)Math.Round(
                            b.CashIncome *
                            (b.VariableExpenses / (double)100), MidpointRounding.AwayFromZero) + b.CashExpense) * buisness.BranchCount;
            }
            else
            {
                user.CashIncome += b.CashExpense;
            }

            user.FreeTime += b.RequireTime * buisness.BranchCount;
            //user.Energy -= BuisnessConstParamters.EnergyToSale;

            _buisnessRepository.Remove(buisness);

            _userRepository.Update(user);

            return GoToMenu(chatId, data);
        }

        MessageToSend ProcessButtonNo(long chatId, TransmittedData data)
        {
            return GoToMenu(chatId, data);
        }
    }


    [BotMethod(State.InRoom.BuisnessCategory.SaleOne, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForSaleOne(TransmittedInfo info)
    {
        var buisness = info.Data.Storage.GetOrThrow<UserBuisness>(ConstantsStorage.Buisness);

        var user = _userRepository.GetByIdOrThrow(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackData.Yes, ProcessButtonYes),
            new (CallbackData.No, ProcessButtonNo)
        });


        MessageToSend ProcessButtonYes(long chatId, TransmittedData data)
        {
            var difCash = user.Cash - buisness.Buisness.Cost;
            var difTime = user.FreeTime - buisness.Buisness.RequireTime;
            //var difEnergy = user.Energy - BuisnessConstParamters.EnergyToSale;

            if (difCash < 0 || difTime < 0/* || difEnergy < 0*/)
            {
                var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

                return new MessageToSend(ReplyText.PurchaseFail(difCash, difTime, null, null), keyboard, false);
            }

            user.Cash -= buisness.Buisness.Cost;
            user.FreeTime -= buisness.Buisness.RequireTime;
            //user.Energy -= BuisnessConstParamters.EnergyToSale;
            if (buisness.OpenSteps >= 0)
            {
                user.CashIncome -= buisness.Buisness.CashIncome - (int)Math.Round(
                            buisness.Buisness.CashIncome *
                            (buisness.Buisness.VariableExpenses / (double)100), MidpointRounding.AwayFromZero) + buisness.Buisness.CashExpense;
            }
            else
            {
                user.CashIncome += buisness.Buisness.CashExpense;
            }

            buisness.BranchCount--;

            _buisnessRepository.Update(buisness);

            _userRepository.Update(user);

            return GoToMenu(chatId, data);
        }

        MessageToSend ProcessButtonNo(long chatId, TransmittedData data)
        {
            return GoToMenu(chatId, data);
        }
    }


    [BotMethod(State.InRoom.BuisnessCategory.InputCountForSaleAny, BotMethodType.Message)]
    public MessageToSend ProcessForSaleAny(TransmittedInfo info)
    {
        var buisness = info.Data.Storage.GetOrThrow<UserBuisness>(ConstantsStorage.Buisness);

        if (!int.TryParse(info.Request, out var number))
        {
            return new MessageToSend("Введите число");
        }

        if (buisness.BranchCount <= number)
        {
            return new MessageToSend($"Введите число не больше {buisness.BranchCount - 1}");
        }
        info.Data.Storage.Add(ConstantsStorage.Count, (short)number);

        info.Data.State = State.InRoom.BuisnessCategory.SaleAny;

        var fakeBuisness = new UserBuisness
        {
            BranchCount = (short)number,
            Buisness = buisness.Buisness
        };

        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Да", CallbackData.Yes),
            InlineKeyboardButton.WithCallbackData("Нет", CallbackData.No));

        return new MessageToSend(ReplyText.BuisnessSale(fakeBuisness, 100, buisness.Buisness.Cost), keyboard, false);
    }


    [BotMethod(State.InRoom.BuisnessCategory.SaleAny, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForSaleAny(TransmittedInfo info)
    {
        var buisness = info.Data.Storage.GetOrThrow<UserBuisness>(ConstantsStorage.Buisness);

        var user = _userRepository.GetByIdOrThrow(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackData.Yes, ProcessButtonYes),
            new (CallbackData.Back, ProcessButtonNo),
            new (CallbackData.No, ProcessButtonNo)
        });


        MessageToSend ProcessButtonYes(long chatId, TransmittedData data)
        {
            // todo вынести в метод
            var count = info.Data.Storage.GetOrThrow<short>(ConstantsStorage.Count);

            var difCash = user.Cash - buisness.Buisness.Cost * count;
            var difTime = user.FreeTime - buisness.Buisness.RequireTime * count;
            //var difEnergy = user.Energy - BuisnessConstParamters.EnergyToSale;

            if (difCash < 0 || difTime < 0/* || difEnergy < 0*/)
            {
                var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

                return new MessageToSend(ReplyText.PurchaseFail(difCash, difTime, null, null), keyboard, false);
            }

            user.Cash -= buisness.Buisness.Cost * count;
            user.FreeTime -= buisness.Buisness.RequireTime * count;
            //user.Energy -= BuisnessConstParamters.EnergyToSale;

            if (buisness.OpenSteps >= 0)
            {
                user.CashIncome -= (buisness.Buisness.CashIncome - (int)Math.Round(
                            buisness.Buisness.CashIncome *
                            (buisness.Buisness.VariableExpenses / (double)100), MidpointRounding.AwayFromZero) + buisness.Buisness.CashExpense) * count;
            }
            else
            {
                user.CashIncome += buisness.Buisness.CashExpense * count;
            }

            buisness.BranchCount -= count;

            _buisnessRepository.Update(buisness);

            _userRepository.Update(user);

            return GoToMenu(chatId, data);
        }

        MessageToSend ProcessButtonNo(long chatId, TransmittedData data)
        {
            return GoToMenu(chatId, data);
        }
    }
    #endregion
    #region Add

    public MessageToSend GoToAddMenu(long chatId, TransmittedData data, Buisness buisness)
    {
        data.Storage.Add(ConstantsStorage.Buisness, buisness);

        data.State = State.InRoom.BuisnessCategory.Add.ChooseAction;

        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Купить", CallbackData.BuisnessCategory.BuyCard),
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.BackToMenu));

        return new MessageToSend("Как хотите использовать?", keyboard);
    }


    [BotMethod(State.InRoom.BuisnessCategory.Add.ChooseAction, BotMethodType.Callback)]
    public MessageToSend ProcessAdd(TransmittedInfo info)
    {
        var buisness = info.Data.Storage.GetOrThrow<Buisness>(ConstantsStorage.Buisness);

        if (info.Request == CallbackData.BackToMenu)
        {
            //return GoToMenu(info.ChatId, info.Data);
            return InRoomService.GoToOpenCard(info.ChatId, info.Data);
        }
        else if (info.Request == CallbackData.Back) // TODO проверить это, если заказчик объявится
        {
            return GoToAddMenu(info.ChatId, info.Data, buisness);
        }

        var user = _userRepository.GetByIdOrThrow(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId));

        var difCash = user.Cash - buisness.Cost;
        var difTime = user.FreeTime - buisness.RequireTime;
        //var difIncome = user.CashIncome - buisness.CashExpense;
        var difEnergy = user.Energy - BuisnessConstParamters.EnergyToOpenBrench;

        if (difCash < 0 || difTime < 0/* || difIncome < 0*/ || difEnergy < 0)
        {
            var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
        InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

            return new MessageToSend(ReplyText.PurchaseFail(difCash, difTime, null, difEnergy), keyboard, false);
        }

        user.Cash -= buisness.Cost;
        user.FreeTime -= buisness.RequireTime;
        user.Energy -= BuisnessConstParamters.EnergyToOpenBrench;
        user.CashIncome -= buisness.CashExpense;

        _buisnessRepository.AddNew(new UserBuisness
        {
            UserId = user.Id,
            BranchCount = 1,
            BuisnessId = buisness.Id,
            OpenSteps = -BuisnessConstParamters.StepsToOpen
        });

        _userRepository.Update(user);

        return GoToMenu(info.ChatId, info.Data);
    }

    #endregion
    #region Service
    public void ForceAddBuisness(Model.User user, Buisness buisness)
    {
        _buisnessRepository.AddNew(new UserBuisness
        {
            UserId = user.Id,
            BranchCount = 1,
            BuisnessId = buisness.Id,
            OpenSteps = 1
        });

        var b = buisness;

        user.FreeTime -= buisness.RequireTime;
        user.CashIncome += (b.CashIncome - (int)Math.Round(
                            b.CashIncome *
                            (b.VariableExpenses / (double)100), MidpointRounding.AwayFromZero) 
                            - b.CashExpense);
    }

    public List<UserBuisness> GetUserBuisnessList(int userId)
    {
        return _buisnessRepository.GetUserBuisnessList(userId);
    }

    public void UpdateUserBuisnessList(List<UserBuisness> userBuisnesses)
    {
        _buisnessRepository.UpdateRange(userBuisnesses);
    }

    public bool TryGetBuisness(string id, out Buisness buisness)
    {
        return _buisnessRepository.TryGetBuisness(id, out buisness);
    }
    #endregion
}
