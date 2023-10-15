using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

using TelegramFinancialGameBot.Data;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Model;

internal class DreamRepository
{
    private DatabaseContext _dbContext;

    public DreamRepository()
    {
        _dbContext = DbContextFactory.CreateDbContext();
    }

    public bool TryGet(string code, out Dream dream)
    {
        dream = _dbContext.Dreams.Where(r => r.Id == code)
            .FirstOrDefault();

        return dream != null;
    }

    public bool TryGetUserDreamExpectation(int userId, out UserDreamExpectation userDreamExpectation)
    {
        userDreamExpectation = _dbContext.UserDreamExpectations.Where(d => d.UserId == userId)
            .Include(d => d.Dream)
            .FirstOrDefault();

        return userDreamExpectation != null;
    }

    public void AddUserDreamExpectation(UserDreamExpectation dreamExpectation)
    {
        _dbContext.UserDreamExpectations.Add(dreamExpectation);
        _dbContext.SaveChanges();
    }

    public void UpdateUserDreamExpectation(UserDreamExpectation dreamExpectation)
    {
        _dbContext.UserDreamExpectations.Update(dreamExpectation);
        _dbContext.SaveChanges();
    }

    public void RemoveDreamExpectation(UserDreamExpectation dreamExpectation)
    {
        _dbContext.UserDreamExpectations.Remove(dreamExpectation);
        _dbContext.SaveChanges();
    }
}
