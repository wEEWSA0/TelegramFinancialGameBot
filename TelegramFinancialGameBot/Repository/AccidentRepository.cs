using Microsoft.EntityFrameworkCore;

using TelegramFinancialGameBot.Data;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Model;

internal class AccidentRepository
{
    private DatabaseContext _dbContext;

    public AccidentRepository()
    {
        _dbContext = DbContextFactory.CreateDbContext();
    }

    public bool TryGet(string id, out Accident accident)
    {
        accident = _dbContext.Accidents.Where(r => r.Id == id)
            .FirstOrDefault();

        return accident != null;
    }

    public List<UserAccident> GetUserAccidentsList(int userId)
    {
        return _dbContext.UserAccidents.Where(r => r.UserId == userId)
            .Include(w => w.Accident)
            .ToList();
    }

    public List<UserAccident> GetUserAccidentsListWithType(int userId, AccidentType type)
    {
        return _dbContext.UserAccidents.Where(r => r.UserId == userId)
            .Include(w => w.Accident)
            .Where(r => r.Accident.Type == type)
            .ToList();
    }

    public void AddUserAccident(UserAccident userAccident)
    {
        _dbContext.UserAccidents.Add(userAccident);
        _dbContext.SaveChanges();
    }

    public void UpdateUserAccident(UserAccident userAccident)
    {
        _dbContext.UserAccidents.Update(userAccident);
        _dbContext.SaveChanges();
    }

    public void RemoveUserAccident(UserAccident userAccident)
    {
        _dbContext.UserAccidents.Remove(userAccident);
        _dbContext.SaveChanges();
    }

    public void UpdateUserAccidentList(List<UserAccident> userAccidents)
    {
        _dbContext.UserAccidents.UpdateRange(userAccidents);
        _dbContext.SaveChanges();
    }
}
