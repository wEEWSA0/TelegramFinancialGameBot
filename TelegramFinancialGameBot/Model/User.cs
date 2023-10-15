using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace TelegramFinancialGameBot.Model;

[Index(nameof(AccountChatId), nameof(RoomId), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }

    public long AccountChatId { get; set; }
    public Account Account { get; set; } = null!;

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public string? DreamId { get; set; }
    public virtual Dream? Dream { get; set; }
    public bool CompleteDream { get; set; }

    public int Cash { get; set; }
    public int FreeTime { get; set; }
    public int CashIncome { get; set; }
    public short Energy { get; set; }
    public bool FinishedStep { get; set; }
}
