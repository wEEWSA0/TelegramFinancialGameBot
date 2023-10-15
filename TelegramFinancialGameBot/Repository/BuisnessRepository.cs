using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TelegramFinancialGameBot.Data;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Model;

internal class BuisnessRepository
{
    private DatabaseContext _dbContext;

    public BuisnessRepository()
    {
        _dbContext = DbContextFactory.CreateDbContext();
    }

    public bool TryGetBuisness(string buisnessId, out Buisness buisness)
    {
        buisness = _dbContext.Buisnesses.Where(r => r.Id == buisnessId)
            .FirstOrDefault();

        return buisness != null;
    }

    public bool TryGetUserBuisness(int buisnessId, out UserBuisness buisness)
    {
        buisness = _dbContext.UserBuisnesses.Where(r => r.Id == buisnessId)
            .Include(b => b.Buisness)
            //.Include(b => b.BuisnessManagerStaff)
            //.Include(b => b.BuisnessRegionalDirectors)
            .Include(b => b.FinancialDirector)
            .Include(b => b.GeneralDirector)
            .FirstOrDefault();

        return buisness != null;
    }

    public List<UserBuisness> GetUserBuisnessList(int userId)
    {
        return _dbContext.UserBuisnesses.Where(w => w.UserId == userId)
            .Include(b => b.Buisness)
            .Include(b => b.FinancialDirector)
            .Include(b => b.GeneralDirector)
            .ToList();
    }

    public void AddNew(UserBuisness userBuisness)
    {
        _dbContext.UserBuisnesses.Add(userBuisness);
        _dbContext.SaveChanges();
    }

    public void Update(UserBuisness userBuisness)
    {
        _dbContext.UserBuisnesses.Update(userBuisness);
        _dbContext.SaveChanges();
    }

    public void UpdateRange(List<UserBuisness> userBuisnesses)
    {
        _dbContext.UserBuisnesses.UpdateRange(userBuisnesses);
        _dbContext.SaveChanges();
    }

    public void Remove(UserBuisness userBuisness)
    {
        _dbContext.UserBuisnesses.Remove(userBuisness);
        _dbContext.SaveChanges();
    }
}
