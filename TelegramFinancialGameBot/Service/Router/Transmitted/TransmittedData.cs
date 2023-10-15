using TelegramFinancialGameBot.Service.Router.Transmitted;

namespace TelegramBotShit.Router.Transmitted;

public class TransmittedData
{
    public int State { get; set; }
    public DataStorage Storage { get; }

    public TransmittedData()
    {
        State = TelegramFinancialGameBot.Service.Router.Transmitted.State.OutOfRoom.CmdStart; 
        Storage = new DataStorage();
    }
}