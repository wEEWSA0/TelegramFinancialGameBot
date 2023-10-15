using Microsoft.Extensions.Configuration;

using TelegramFinancialGameBot;
using TelegramFinancialGameBot.Data;

var defaultConnection = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();

config["ConnectionStrings:DefaultConnection"] = defaultConnection;

var connectionString = config["ConnectionStrings:DefaultConnection"];

if (connectionString == null || connectionString == "")
{
    connectionString = config["ConnectionStrings:DevelopmentConnection"];

    if (connectionString == null)
    {
        throw new ArgumentNullException(nameof(connectionString));
    }
}

DbContextFactory.ConnectionString = connectionString;

BotStartup.Start();

await ProjectStartup.WaitExit(ProcessExit);

void ProcessExit(object sender, EventArgs e)
{
    Console.WriteLine("Bot will stopped");

    BotStartup.Bot.Stop();

    Console.WriteLine("Bot stopped");
}


// todo заменить в throw на переход в другие логические блоки

/* todo добавить команду /reset для сброса:
 * 1. регистрация пользователя
 * 2. меню выбора комнаты
 * 3. главное меню в комнате
 */