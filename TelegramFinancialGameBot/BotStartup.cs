using TelegramBotShit.Bot;

namespace TelegramFinancialGameBot;

public static class BotStartup
{
    public static Bot Bot { get; set; }

    private const string _myDevelopmentToken = "6282911984:AAH9Eh8bX9gD8QidlLi5HjOo0KVAy9zxIsI";
    private const string _officialToken = "6601163369:AAGfxI3H5GRY6CtfvGNBwQKgFstniKYlHS0";

    public static void Start()
    {

        Bot = new Bot(_myDevelopmentToken); // todo move to configuration

        Bot.Start();

        Console.WriteLine("\nPress 'Ctrl + c' to stop\n");
    }
}
