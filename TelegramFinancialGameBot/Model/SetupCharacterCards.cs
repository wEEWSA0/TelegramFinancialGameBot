using Microsoft.EntityFrameworkCore;

namespace TelegramFinancialGameBot.Model;

[PrimaryKey(nameof(SetupCharacterId), nameof(Card))]
public class SetupCharacterCards
{
    public string SetupCharacterId { get; set; } = null!;
    public SetupCharacter SetupCharacter { get; set; } = null!;

    public string Card { get; set; } = null!;
    public string? AdditionalInfo { get; set; }
}
