using Microsoft.EntityFrameworkCore;

namespace TelegramFinancialGameBot.Model;

[PrimaryKey(nameof(UserId), nameof(DreamId))]
public class UserDreamExpectation
{
    public int UserId { get; set; }
    public User User { get; set; }
    public string DreamId { get; set; } = null!;
    public Dream Dream { get; set; }

    public short Steps { get; set; }
}
