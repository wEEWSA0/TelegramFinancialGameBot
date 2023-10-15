using Microsoft.EntityFrameworkCore;

namespace TelegramFinancialGameBot.Model;

[PrimaryKey(nameof(UserId), nameof(AccidentId))]
public class UserAccident
{
    public int UserId { get; set; }
    public User User { get; set; }
    public string AccidentId { get; set; } = null!;
    public Accident Accident { get; set; } = null!;

    public short CurrentStep { get; set; }
}
