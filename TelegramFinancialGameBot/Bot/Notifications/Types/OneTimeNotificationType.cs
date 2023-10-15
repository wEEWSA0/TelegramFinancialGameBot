namespace TelegramBotShit.Bot.Notifications.Types;

public class OneTimeNotificationType : INotificationType
{
    public bool IsTimeForSendNotification(DateTime currentDateTime)
    {
        return true;
    }

    public bool IsNotificationExpired(DateTime currentDateTime)
    {
        return true;
    }
}