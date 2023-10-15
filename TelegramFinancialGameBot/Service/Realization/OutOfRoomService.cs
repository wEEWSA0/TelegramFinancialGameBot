using NLog;

using Telegram.Bot.Types.ReplyMarkups;

using TelegramBotShit.Bot;
using TelegramBotShit.Bot.Buttons;
using TelegramBotShit.Router.Transmitted;

using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Service.Attributes;
using TelegramFinancialGameBot.Service.Realization;
using TelegramFinancialGameBot.Service.Router.Transmitted;
using TelegramFinancialGameBot.Util;
using TelegramFinancialGameBot.Util.Keyboard;
using TelegramFinancialGameBot.Util.Reply;

namespace TelegramBotShit.Service.ServiceRealization;

[BotMethodsClass]
public class OutOfRoomService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    private static OutOfRoomStateReplyStorage ReplyText = ReplyStorage.OutOfRoom;

    private AccountRepository _accountRepository;
    private RoomRepository _roomRepository;
    private UserRepository _userRepository;

    public OutOfRoomService()
    {
        _accountRepository = new();
        _roomRepository = new();
        _userRepository = new();
    }

    #region InputAndChooseMethods
    
    // Здесь происходит setup пользователя
    [BotMethod(State.OutOfRoom.CmdStart, BotMethodType.Message)]
    [BotMethod(State.OutOfRoom.CmdStart, BotMethodType.Callback)]
    public MessageToSend ProcessStart(TransmittedInfo info)
    {
        if (_accountRepository.TryGet(info.ChatId, out Account account))
        {
            RealizationUtil.AddMessageToStack(info.ChatId, new MessageToSend("Бот перезагружен"));

            return GoToNearestMenu(info, account);
        }

        if (info.Request != ReceiveCommandsStorage.Start)
        {
            return new MessageToSend(ReplyText.ErrorInput);
        }
        else
        {
            info.Data.State = State.OutOfRoom.InputName;

            SendOfficialArchoredMessage(info.ChatId);

            return new MessageToSend(ReplyText.InputYourName);
        }
    }


    [BotMethod(State.OutOfRoom.InputName, BotMethodType.Message)]
    public MessageToSend ProcessInputName(TransmittedInfo info)
    {
        info.Data.Storage.Add(ConstantsStorage.AccountName, info.Request);

        info.Data.State = State.OutOfRoom.InputtedNameCorrect;

        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Да", CallbackQueryStorage.OutOfRoom.YesForInputName),
            InlineKeyboardButton.WithCallbackData("Нет", CallbackQueryStorage.OutOfRoom.NoForInputName));

        return new MessageToSend(ReplyText.InputUserName(info.Request), keyboard);
    }


    // Здесь создается Room
    [BotMethod(State.OutOfRoom.InputRoomNameForCreateIt, BotMethodType.Message)]
    public MessageToSend ProcessInputRoomNameForCreateIt(TransmittedInfo info)
    {
        if (_roomRepository.TryGet(info.Request, out Room foundRoom))
        {
            return new MessageToSend(ReplyText.RoomAlreadyExist);
        }

        var room = new Room()
        {
            Name = info.Request,
            Step = 1,
            VictoryConditions = new VictoryConditions(),
            OwnerChatId = info.ChatId
        };

        // TODO баг иногда
        // TODO постоянный баг с Accidents repository
        try
        {
            room = _roomRepository.AddNew(room);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            Logger.Error(ex);

            return MessageToSend.Empty;
        }

        info.Data.Storage.Add(ConstantsStorage.RoomId, room.Id);
        info.Data.Storage.Add(ConstantsStorage.RoomName, room.Name);
        info.Data.Storage.Add(ConstantsStorage.VictoryConditions, room.VictoryConditions);

        return GoToSetupRoom(info.Data, room.VictoryConditions);
    }


    [BotMethod(State.OutOfRoom.InputRoomNameForJoinInIt, BotMethodType.Message)]
    public MessageToSend ProcessInputRoomNameForJoinInIt(TransmittedInfo info)
    {
        if (!_roomRepository.TryGet(info.Request, out Room room))
        {
            return new MessageToSend(ReplyText.RoomNotFound);
        }

        if (room.VictoryConditions == null)
        {
            return new MessageToSend(ReplyText.FoundRoomWithoutVictoryConditions);
        }

        info.Data.Storage.Add(ConstantsStorage.RoomId, room.Id);
        info.Data.Storage.Add(ConstantsStorage.RoomName, room.Name);

        return GoToJoinInRoom(info, room.VictoryConditions);
    }

    #endregion
    #region ButtonsMethods

    [BotMethod(State.OutOfRoom.InputtedNameCorrect, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForInputtedNameCorrect(TransmittedInfo info)
    {
        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackQueryStorage.OutOfRoom.YesForInputName, ProcessButtonYesForInputName),
            new (CallbackQueryStorage.OutOfRoom.NoForInputName, ProcessButtonNoForInputName)
        });


        MessageToSend ProcessButtonYesForInputName(long chatId, TransmittedData data)
        {
            var name = (string)data.Storage.GetOrThrow(ConstantsStorage.AccountName);

            var user = new Account()
            {
                ChatId = chatId,
                Name = name,
                isLink = true
            };

            _accountRepository.AddNew(user);

            return GoToCreateOrJoinRoom(data);
        }

        MessageToSend ProcessButtonNoForInputName(long chatId, TransmittedData data)
        {
            data.State = State.OutOfRoom.InputName;

            BotMessageManager.Instance.GetHistory(chatId).DeleteLastMessage();

            return new MessageToSend(ReplyText.InputYourName);
        }
    }


    [BotMethod(State.OutOfRoom.CreateOrJoinRoom, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForCreateOrJoinRoom(TransmittedInfo info)
    {
        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackQueryStorage.OutOfRoom.CreateRoom, ProcessButtonCreateRoom),
            new (CallbackQueryStorage.OutOfRoom.JoinInRoom, ProcessButtonJoinRoom)
        });


        MessageToSend ProcessButtonCreateRoom(long chatId, TransmittedData data)
        {
            return GoToInputRoomName(data, State.OutOfRoom.InputRoomNameForCreateIt);
        }

        MessageToSend ProcessButtonJoinRoom(long chatId, TransmittedData data)
        {
            return GoToInputRoomName(data, State.OutOfRoom.InputRoomNameForJoinInIt);
        }
    }


    [BotMethod(State.OutOfRoom.InputRoomNameForCreateIt, BotMethodType.Callback)]
    [BotMethod(State.OutOfRoom.InputRoomNameForJoinInIt, BotMethodType.Callback)]
    public MessageToSend ProcessButtonBack(TransmittedInfo info)
    {
        BotMessageManager.Instance.GetHistory(info.ChatId).DeleteLastMessage();

        return GoToCreateOrJoinRoom(info.Data);
    }

    #endregion
    #region SupportMethods

    private static async void SendOfficialArchoredMessage(long chatId)
    {
        var message = new MessageToSend(ReplyText.ArchoredMessage);

        await BotNotificationSender.Instance.SendAnchoredNotificationMessage(message, chatId);
    }

    public static MessageToSend GoToCreateOrJoinRoom(TransmittedData data)
    {
        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Создать комнату", CallbackQueryStorage.OutOfRoom.CreateRoom),
            InlineKeyboardButton.WithCallbackData("Войти в комнату", CallbackQueryStorage.OutOfRoom.JoinInRoom));

        data.State = State.OutOfRoom.CreateOrJoinRoom;

        return new MessageToSend(ReplyText.CreateOrJoinRoom, keyboard, false);
    }

    private static MessageToSend GoToInputRoomName(TransmittedData data, int state)
    {
        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Назад", CallbackQueryStorage.OutOfRoom.BackInInputName));

        data.State = state;

        return new MessageToSend(ReplyText.InputRoomName, keyboard, false);
    }

    public static MessageToSend GoToSetupRoom(TransmittedData data, VictoryConditions conditions)
    {
        data.State = State.SetupRoom.MainMenu;

        var callback = CallbackQueryStorage.SetupRoom;

        var dream = conditions.ShouldDreamBeCompleted ? "Без мечты" : "С мечтой";

        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Изменить денежный поток", callback.CashIncomeCondition),
            InlineKeyboardButton.WithCallbackData("Изменить свободное время", callback.FreeTimeCondition),
            //InlineKeyboardButton.WithCallbackData("Изменить банковские счета", callback.BankCondition),
            InlineKeyboardButton.WithCallbackData(dream, callback.DreamCondition),
            InlineKeyboardButton.WithCallbackData("Оставить текущие условия", callback.SetupCurrentConditions),
            InlineKeyboardButton.WithCallbackData("Назад", callback.Back));

        return new MessageToSend(ReplyStorage.SetupRoom.SetupVictoryConditions(conditions), keyboard, false);
    }

    public static MessageToSend GoToJoinInRoom(TransmittedInfo info, VictoryConditions conditions)
    {
        info.Data.State = State.InRoom.SetupCharacter.CreateCharacter;

        /* устарело, теперь только персонажи
        var keyboard = BotKeyboardCreator.Instance.GetKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Начать с нуля", CallbackQueryStorage.InRoom.StartWithoutCharacter));*/

        RealizationUtil.AddMessageToStack(info.ChatId, new MessageToSend(ReplyStorage.InRoom.FirstJoinInRoom(info.Request, conditions)));

        return new MessageToSend(ReplyStorage.InRoom.InputCharacterCode, false);
    }

    public MessageToSend GoToNearestMenu(TransmittedInfo info, Account account)
    {
        var archCount = BotMessageManager.Instance.GetHistory(info.ChatId).AnchoredMessagesCount;

        if (archCount == 0)
        {
            SendOfficialArchoredMessage(info.ChatId);
        }

        info.Data.Storage.Add(ConstantsStorage.AccountName, account.Name);

        if (_userRepository.TryGetUserRooms(info.ChatId, out Room[] rooms))
        {
            if (rooms.Length >= 2)
            {
                Logger.Warn($"Account '{account.Name}' consist in {rooms.Length} rooms. It can't be");

                return GoToCreateOrJoinRoom(info.Data);
            }

            if (!_roomRepository.TryGet(rooms.First().Id, out Room room))
            {
                Logger.Error($"Wtf exception. It can't be");

                return GoToCreateOrJoinRoom(info.Data);
            }

            if (!_userRepository.TryGetWithDreamIfOnlyOneUserInRoomExist(info.ChatId, out User user))
            {
                Logger.Error($"Wtf exception. It can't be");

                return GoToCreateOrJoinRoom(info.Data);
            }

            info.Data.Storage.Add(ConstantsStorage.RoomId, room.Id);
            info.Data.Storage.Add(ConstantsStorage.RoomName, room.Name);
            info.Data.Storage.Add(ConstantsStorage.VictoryConditions, room.VictoryConditions);
            info.Data.Storage.Add(ConstantsStorage.UserId, user.Id);
            info.Data.Storage.Add(ConstantsStorage.AccountName, account.Name);

            return InRoomService.GoToMainMenu(info.ChatId, info.Data, user, user.Dream, room);
        }

        return GoToCreateOrJoinRoom(info.Data);
    }

    #endregion
}