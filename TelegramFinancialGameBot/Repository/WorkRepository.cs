using Microsoft.EntityFrameworkCore;

using TelegramFinancialGameBot.Data;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Model;

internal class WorkRepository
{
    private DatabaseContext _dbContext;

    public WorkRepository()
    {
        _dbContext = DbContextFactory.CreateDbContext();
    }

    public List<UserWorkPosition> GetUserWorkPositionList(int userId)
    {
        return _dbContext.UserWorkPositions.Where(w => w.UserId == userId)
            .Include(w => w.WorkPosition)
            .Include(w => w.WorkPosition.Work).ToList();
    }

    public bool TryGetWorkPositionWithZeroExpirience(string workId, out WorkPosition position)
    {
        position = _dbContext.WorkPositions.Where(w => w.WorkId == workId && w.ExpirienceRequire == 0)
            .FirstOrDefault();

        return position != null;
    }

    public void AddNew(UserWorkPosition workPosition)
    {
        _dbContext.UserWorkPositions.Add(workPosition);
        _dbContext.SaveChanges();
    }

    public void Update(List<UserWorkPosition> userWorkPositions)
    {
        _dbContext.UserWorkPositions.UpdateRange(userWorkPositions);
        _dbContext.SaveChanges();
    }

    // TryGetUserWork, TryGetUserWorks
    //public bool TryGetWorkOnly(string workId, out Work work)
    //{
    //    work = _dbContext.Works.Where(r => r.WorkId == workId).FirstOrDefault();

    //    return work != null;
    //}

    //public bool TryGetWorkPosition(string workId, int workId, out Work work)
    //{
    //    work = _dbContext.WorkPositions.Where(r => r.WorkId == workId && r.Income).FirstOrDefault();

    //    return work != null;
    //}
}
