namespace TelegramBotShit.Bot.Notifications;

public class RegularNotification : Notification
{
    // Класс не работает, будет реализован позже
    public int PeriodInDays { get; private set; }
    public Func<MessageToSend> _messageToSendGiver;
    
    // todo реализовать регулярные уведомления (приоритет: малый)
    
    public RegularNotification(Func<MessageToSend> messageToSendGiver, int periodInDays)
    {
        _messageToSendGiver = messageToSendGiver;
        PeriodInDays = periodInDays;
    }

    public override async void Send()
    {
        if (_recieverList.Count == 0)
        {
            Logger.Error("Нельзя отправить уведомление 0 пользователям");
            throw new Exception();
        }
        
        _message = _messageToSendGiver.Invoke(); // todo invoke => ?invoke
    
        Date.AddDays(PeriodInDays);
        
        var notificationSender = BotNotificationSender.Instance;
        
        for (int i = 0; i < _recieverList.Count; i++)
        {
            await notificationSender.SendNotificationMessage(_message, _recieverList[i]);
        }
    }
    
    /*
     * Класс постоянного уведомления
     */
}
