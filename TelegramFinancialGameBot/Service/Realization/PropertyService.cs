using Azure.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types;
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
public class PropertyService
{
    private static InRoomStateReplyStorage ReplyText = ReplyStorage.InRoom;
    private static InRoomStateCallbackQueryStorage CallbackData = CallbackQueryStorage.InRoom;

    private UserRepository _userRepository;
    private PropertyRepository _propertyRepository;

    public PropertyService()
    {
        _userRepository = new UserRepository();
        _propertyRepository = new PropertyRepository();
    }

    #region Menu

    public MessageToSend GoToMenu(long chatId, TransmittedData data)
    {
        if (!_propertyRepository.TryGetUserProperties(data.Storage.GetOrThrow<int>(ConstantsStorage.UserId), out var properties))
        {
            var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

            data.State = State.InRoom.PropertyCategory.Info;

            return new MessageToSend("У вас нет недвижимости", keyboard, false);
        }

        var dynamicKeyboard = GetPropertiesKeyboard(properties.ToArray());

        data.State = State.InRoom.PropertyCategory.Info;

        return new MessageToSend(ReplyText.YourProperties, dynamicKeyboard, false);
    }


    [BotMethod(State.InRoom.PropertyCategory.Info, BotMethodType.Callback)]
    public MessageToSend ProcessChoosedProperty(TransmittedInfo info)
    {
        if (info.Request == CallbackData.Back)
        {
            if (!_userRepository.TryGetById(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId), out var user))
            {
                throw new Exception();
            }

            return InRoomService.GoToMainMenuWithRemoveMessageHistory(info.ChatId, info.Data, user, user.Dream, user.Room);
        }

        var par = info.Request.Split(',');

        if (!_propertyRepository.TryGetUserProperty(par[1], int.Parse(par[0]), out var property))
        {
            throw new Exception();
        }

        InlineKeyboardMarkup keyboard;

        if (property.IsOwner)
        {
            if (property.UsesAsHome)
            {
                keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("Съехать", CallbackData.PropertyCategory.MoveTo),
                    InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back)
                    );
            }
            else
            {
                keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("Продать", CallbackData.PropertyCategory.Sale),
                    InlineKeyboardButton.WithCallbackData("Переехать", CallbackData.PropertyCategory.MoveIn),
                    InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back)
                    );
            }
        }
        else
        {
            keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("Съехать", CallbackData.PropertyCategory.MoveTo),
                    InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back)
                    );
        }

        info.Data.State = State.InRoom.PropertyCategory.ChooseAction;

        info.Data.Storage.Add(ConstantsStorage.Property, property);

        return new MessageToSend(ReplyText.PropertyInfo(property), keyboard, false);
    }


    [BotMethod(State.InRoom.PropertyCategory.ChooseAction, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForActionWithProperty(TransmittedInfo info)
    {
        var property = info.Data.Storage.GetOrThrow<UserProperty>(ConstantsStorage.Property);

        if (!_userRepository.TryGet(info.ChatId, info.Data.Storage.GetOrThrow<int>(ConstantsStorage.RoomId), out var user))
        {
            throw new Exception();
        }

        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackData.PropertyCategory.Sale, ProcessButtonSale),
            new (CallbackData.PropertyCategory.MoveTo, ProcessButtonMoveTo),
            new (CallbackData.PropertyCategory.MoveIn, ProcessButtonMoveIn),
            new (CallbackData.Back, ProcessButtonBack)
        });

        MessageToSend ProcessButtonSale(long chatId, TransmittedData data)
        {
            user.Cash += property.Property.Cost;
            user.CashIncome -= property.Property.RentCashIncome;
            user.FreeTime += property.Property.TimeExpense;

            _propertyRepository.Remove(property);

            _userRepository.Update(user);

            RealizationUtil.AddMessageToStack(chatId, new MessageToSend(ReplyText.PropertySaled(property.Property.Name, property.Property.Cost)));

            return GoToMenu(chatId, data);
        }

        MessageToSend ProcessButtonMoveTo(long chatId, TransmittedData data)
        {
            if (!_propertyRepository.TryGetUserProperties(data.Storage.GetOrThrow<int>(ConstantsStorage.UserId), out var properties))
            {
                throw new Exception();
            }

            if (properties.Count < 2)
            {
                RealizationUtil.AddMessageToStack(chatId, new MessageToSend("У вас нет недвижимости, в которую можно переехать"));

                return GoToMenu(chatId, data);
            }

            properties.Remove(property);

            var dynamicKeyboard = GetPropertiesKeyboard(properties.ToArray());

            data.State = State.InRoom.PropertyCategory.MoveTo;

            return new MessageToSend(ReplyText.ChoosePropertiesToMoveIn, dynamicKeyboard, false);
        }

        MessageToSend ProcessButtonMoveIn(long chatId, TransmittedData data)
        {
            if (!_propertyRepository.TryGetUserPropertyWhichUsesAsHome(data.Storage.GetOrThrow<int>(ConstantsStorage.UserId), out var propertyAsHome))
            {
                property.UsesAsHome = true;

                RealizationUtil.AddMessageToStack(chatId, new MessageToSend(ReplyText.MoveToFirstProperty(!property.IsOwner, property.Property.Name), false));

                _propertyRepository.Update(property);

                return InRoomService.GoToMainMenu(chatId, data, user, user.Dream, user.Room);
            }

            return MoveToProperty(info.ChatId, info.Data, property, propertyAsHome);
        }

        MessageToSend ProcessButtonBack(long chatId, TransmittedData data)
        {
            //if (!_userRepository.TryGetWithDream(info.ChatId, info.Data.Storage.GetOrThrow<int>(ConstantsStorage.RoomId), out var user))
            //{
            //    throw new Exception();
            //}

            //return InRoomService.GoToMainMenu(chatId, data, user, user.Dream, user.Room).With(false);
            return GoToMenu(chatId, data);
        }
    }


    [BotMethod(State.InRoom.PropertyCategory.MoveTo, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForMoveToProperty(TransmittedInfo info)
    {
        var propertyToLeft = info.Data.Storage.GetOrThrow<UserProperty>(ConstantsStorage.Property);

        if (info.Request == CallbackData.Back)
        {
            var userId = info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId);

            var request = $"{userId},{propertyToLeft.PropertyId}"; // todo можно перенести это в static метод

            return ProcessChoosedProperty(new TransmittedInfo(info.ChatId, info.Data, request));
        }

        var par = info.Request.Split(',');

        if (!_propertyRepository.TryGetUserProperty(par[1], int.Parse(par[0]), out var property))
        {
            throw new ArgumentException(info.Request);
        }

        return MoveToProperty(info.ChatId, info.Data, property, propertyToLeft);
    }

    private MessageToSend MoveToProperty(long chatId, TransmittedData data, UserProperty propertyToBeHome, UserProperty propertyToNotBeHome)
    {
        if (!_userRepository.TryGet(chatId, data.Storage.GetOrThrow<int>(ConstantsStorage.RoomId), out var user))
        {
            throw new Exception();
        }

        propertyToBeHome.UsesAsHome = true;
        propertyToNotBeHome.UsesAsHome = false;

        RealizationUtil.AddMessageToStack(chatId, new MessageToSend(ReplyText.LeftProperty(!propertyToNotBeHome.IsOwner, propertyToBeHome.Property.Name), false));

        if (!propertyToNotBeHome.IsOwner)
        {
            _propertyRepository.Remove(propertyToNotBeHome);
        }
        else
        {
            _propertyRepository.Update(propertyToNotBeHome);
        }

        _propertyRepository.Update(propertyToBeHome);

        return InRoomService.GoToMainMenu(chatId, data, user, user.Dream, user.Room);
    }

    private static InlineKeyboardMarkup GetPropertiesKeyboard(UserProperty[] properties)
    {
        List<InlineKeyboardButton> buttons = new();

        foreach (UserProperty property in properties)
        {
            var additionalText = property.IsOwner ? property.UsesAsHome
                ? " (Пассив)" : " (Актив)" : " (Снимаете)";
            buttons.Add(
                InlineKeyboardButton.WithCallbackData(property.Property.Name + additionalText, $"{property.UserId},{property.PropertyId}")
                );
        }

        buttons.Add(InlineKeyboardButton.WithCallbackData("Назад", CallbackQueryStorage.InRoom.Back));

        return BotKeyboardCreator.Instance.GetKeyboardMarkup(buttons.ToArray());
    }

    #endregion
    #region Add

    public MessageToSend GoToAddMenu(long chatId, TransmittedData data, Property property)
    {
        // Меню купить, снимать, купить и жить
        data.Storage.Add(ConstantsStorage.Property, property);

        data.State = State.InRoom.PropertyCategory.Add.ChooseAction;

        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Купить и сдавать в аренду", CallbackData.PropertyCategory.BuyCard),
            InlineKeyboardButton.WithCallbackData("Купить и жить", CallbackData.PropertyCategory.BuyAndUseAsHomeCard),
            InlineKeyboardButton.WithCallbackData("Снять в аренду", CallbackData.PropertyCategory.RentCard),
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

        return new MessageToSend("Как хотите использовать?", keyboard);
    }


    [BotMethod(State.InRoom.PropertyCategory.Add.ChooseAction, BotMethodType.Callback)]
    public MessageToSend ProcessAddProperty(TransmittedInfo info)
    {
        // Меню купить, снимать, купить и жить
        var property = info.Data.Storage.GetOrThrow<Property>(ConstantsStorage.Property);

        if (!_userRepository.TryGetById(info.Data.Storage.GetOrThrow<int>(ConstantsStorage.UserId), out var user))
        {
            throw new Exception();
        }

        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackData.PropertyCategory.BuyCard, BuyPropertyToRent),
            new (CallbackData.PropertyCategory.BuyAndUseAsHomeCard, BuyPropertyForUsesAsHome),
            new (CallbackData.PropertyCategory.RentCard, RentProperty),
            new (CallbackData.Back, ProcessButtonBackSub)
        });


        MessageToSend BuyPropertyToRent(long chatId, TransmittedData data)
        {
            return BuyProperty(chatId, data, false);
        }

        MessageToSend BuyPropertyForUsesAsHome(long chatId, TransmittedData data)
        {
            return BuyProperty(chatId, data, true);
        }

        MessageToSend RentProperty(long chatId, TransmittedData data)
        {
            var difCash = user.CashIncome - property.RentCashIncome - property.CashExpense;
            var difTime = user.FreeTime - property.TimeExpense;
            var difEnergy = user.Energy - property.EnergyCost;

            if (difCash < 0 || difTime < 0 || difEnergy < 0)
            {
                var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

                return new MessageToSend(ReplyText.PurchaseFail(difCash, difTime, null, difEnergy), keyboard, false);
            }

            user.FreeTime -= property.TimeExpense;
            user.CashIncome -= property.RentCashIncome + property.CashExpense;
            user.Energy -= property.EnergyCost;

            _propertyRepository.AddNew(new UserProperty
            {
                UsesAsHome = true,
                IsOwner = false,
                UserId = user.Id,
                PropertyId = property.Id
            });

            _userRepository.Update(user);

            return GoToMenu(chatId, data);
        }

        MessageToSend BuyProperty(long chatId, TransmittedData data, bool usesAsHome)
        {
            var difCash = user.Cash - property.Cost;
            var difTime = user.FreeTime - property.TimeExpense;
            var difIncome = user.CashIncome - property.CashExpense;
            var difEnergy = user.Energy - property.EnergyCost;

            if (difCash < 0 || difTime < 0 || difIncome < 0 || difEnergy < 0)
            {
                var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Назад", CallbackData.Back));

                return new MessageToSend(ReplyText.PurchaseFail(difCash, difTime, difIncome, difEnergy), keyboard, false);
            }

            user.Cash -= property.Cost;
            user.FreeTime -= property.TimeExpense;
            user.Energy -= property.EnergyCost;

            if (!usesAsHome)
            {
                user.CashIncome += property.RentCashIncome;
            }
            else
            {
                user.CashIncome -= property.CashExpense;
            }

            _propertyRepository.AddNew(new UserProperty
            {
                UsesAsHome = false,
                IsOwner = true,
                UserId = user.Id,
                PropertyId = property.Id
            });

            _userRepository.Update(user);

            return GoToMenu(chatId, data);
        }

        MessageToSend ProcessButtonBackSub(long chatId, TransmittedData data)
        {
            return GoToMenu(chatId, data);
        }
    }

    #endregion
    #region Service

    public bool TryGetProperty(string id, out Property property)
    {
        return _propertyRepository.TryGetProperty(id, out property);
    }

    public List<UserProperty> GetUserPropertyList(int userId)
    {
        return _propertyRepository.GetUserPropertyList(userId);
    }

    public void ForceAddProperty(Model.User user, Property property, bool usesAsHome, bool isOwner)
    {
        var userProperty = _propertyRepository.AddNew(new UserProperty
        {
            UserId = user.Id,
            IsOwner = isOwner,
            UsesAsHome = usesAsHome,
            PropertyId = property.Id
        });

        user.FreeTime -= userProperty.Property.TimeExpense;

        if (userProperty.IsOwner)
        {
            if (userProperty.UsesAsHome)
            {
                user.CashIncome -= userProperty.Property.CashExpense;
            }
            else
            {
                user.CashIncome += userProperty.Property.RentCashIncome;
            }
        }
        else
        {
            user.CashIncome -= userProperty.Property.RentCashIncome;
        }
    }

    #endregion
}
