using NLog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types.ReplyMarkups;

using TelegramBotShit.Bot;
using TelegramBotShit.Bot.Buttons;
using TelegramBotShit.Router.Transmitted;
using TelegramBotShit.Service.ServiceRealization;

using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Model;
using TelegramFinancialGameBot.Service.Attributes;
using TelegramFinancialGameBot.Service.Router.Transmitted;
using TelegramFinancialGameBot.Util;
using TelegramFinancialGameBot.Util.Keyboard;
using TelegramFinancialGameBot.Util.Reply;

namespace TelegramFinancialGameBot.Service.Realization;

[BotMethodsClass]
internal class SetupRoomService
{
    private static SetupRoomStateReplyStorage ReplyText = ReplyStorage.SetupRoom;
    private static SetupRoomStateCallbackQueryStorage CallbackStorage = CallbackQueryStorage.SetupRoom;

    private RoomRepository _roomRepository;
    private UserRepository _userRepository;

    public SetupRoomService()
    {
        _roomRepository = new RoomRepository();
        _userRepository = new UserRepository();
    }

    [BotMethod(State.SetupRoom.MainMenu, BotMethodType.Callback)]
    public MessageToSend ProcessButtonsForMainMenu(TransmittedInfo info)
    {
        return RealizationUtil.ProcessButtons(info, new CallbackMethod[]
        {
            new (CallbackStorage.Back, ProcessButtonBack),
            new (CallbackStorage.CashIncomeCondition, ProcessButtonChangeCashIncome),
            new (CallbackStorage.FreeTimeCondition, ProcessButtonChangeFreeTime),
            new (CallbackStorage.DreamCondition, ProcessButtonChangeDream),
            new (CallbackStorage.SetupCurrentConditions, ProcessButtonContinue),
        });


        MessageToSend ProcessButtonBack(long chatId, TransmittedData data)
        {
            _roomRepository.TryRemoveByName((string)info.Data.Storage.GetOrThrow(ConstantsStorage.RoomName));

            return OutOfRoomService.GoToCreateOrJoinRoom(data);
        }

        MessageToSend ProcessButtonChangeCashIncome(long chatId, TransmittedData data)
        {
            info.Data.State = State.SetupRoom.ChangeCashIncome;

            return new MessageToSend(ReplyText.ChangeCashIncome, false);
        }

        MessageToSend ProcessButtonChangeFreeTime(long chatId, TransmittedData data)
        {
            info.Data.State = State.SetupRoom.ChangeFreeTime;

            return new MessageToSend(ReplyText.ChangeFreeTime, false);
        }

        MessageToSend ProcessButtonChangeDream(long chatId, TransmittedData data)
        {
            var conditions = (VictoryConditions)info.Data.Storage.GetOrThrow(ConstantsStorage.VictoryConditions);

            conditions.ShouldDreamBeCompleted = !conditions.ShouldDreamBeCompleted;

            return OutOfRoomService.GoToSetupRoom(info.Data, conditions);
        }

        MessageToSend ProcessButtonContinue(long chatId, TransmittedData data)
        {
            var conditions = (VictoryConditions)info.Data.Storage.GetOrThrow(ConstantsStorage.VictoryConditions);

            var roomName = (string)data.Storage.GetOrThrow(ConstantsStorage.RoomName);

            if (!_roomRepository.TryGet(roomName, out Room room))
            {
                throw new Exception($"Room not exist by name: '{roomName}', but should exist in SetupRoom");
            }

            room.VictoryConditions = conditions;

            _roomRepository.Update(room);

            return OutOfRoomService.GoToJoinInRoom(new TransmittedInfo(chatId, data, roomName), conditions);
        }
    }


    [BotMethod(State.SetupRoom.ChangeCashIncome, BotMethodType.Message)]
    public MessageToSend ProcessChangeCashIncome(TransmittedInfo info)
    {
        if (!int.TryParse(info.Request, out int num))
        {
            return new MessageToSend(ReplyText.ChangeCashIncomeError);
        }

        if (num < 0 || num > 1000000000)
        {
            return new MessageToSend(ReplyText.ChangeCashIncomeError);
        }

        var conditions = (VictoryConditions)info.Data.Storage.GetOrThrow(ConstantsStorage.VictoryConditions);

        conditions.CashIncome = num;

        return OutOfRoomService.GoToSetupRoom(info.Data, conditions);
    }


    [BotMethod(State.SetupRoom.ChangeFreeTime, BotMethodType.Message)]
    public MessageToSend ProcessChangeFreeTime(TransmittedInfo info)
    {
        if (!short.TryParse(info.Request, out short num))
        {
            return new MessageToSend(ReplyText.ChangeFreeTimeError);
        }

        if (num < 0 || num > 1000)
        {
            return new MessageToSend(ReplyText.ChangeFreeTimeError);
        }

        var conditions = (VictoryConditions)info.Data.Storage.GetOrThrow(ConstantsStorage.VictoryConditions);

        conditions.RequireTime = num;

        return OutOfRoomService.GoToSetupRoom(info.Data, conditions);
    }
}
