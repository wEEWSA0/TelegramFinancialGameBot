using Microsoft.EntityFrameworkCore;

using Telegram.Bot.Types;

namespace TelegramFinancialGameBot.Model;

[PrimaryKey(nameof(UserId), nameof(KnowledgeId))]
public class UserKnowledge
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string KnowledgeId { get; set; } = null!;
    public Knowledge Knowledge { get; set; } = null!;

    public short TimeToLearn { get; set; }
    public short Experience { get; set; }
}
