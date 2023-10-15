using Microsoft.EntityFrameworkCore;

using TelegramFinancialGameBot.Data;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Model;

internal class SetupCharacterRepository
{
    private DatabaseContext _dbContext;

    public SetupCharacterRepository()
    {
        _dbContext = DbContextFactory.CreateDbContext();
    }

    public bool TryGet(string code, out SetupCharacter character)
    {
        character = _dbContext.SetupCharacters.Where(r => r.Id == code)
            .Include(r => r.Cards).FirstOrDefault();

        return character != null;
    }
}
