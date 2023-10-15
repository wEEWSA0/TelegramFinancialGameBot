using Microsoft.EntityFrameworkCore;

using TelegramFinancialGameBot.Data;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Model;

internal class PropertyRepository
{
    private DatabaseContext _dbContext;

    public PropertyRepository()
    {
        _dbContext = DbContextFactory.CreateDbContext();
    }

    public bool TryGetProperty(string propertyId, out Property property)
    {
        property = _dbContext.Properties.Where(r => r.Id == propertyId)
            .FirstOrDefault();

        return property != null;
    }

    public bool TryGetUserProperty(string propertyId, int userId, out UserProperty property)
    {
        property = _dbContext.UserProperties.Where(r => r.PropertyId == propertyId && r.UserId == userId)
            .Include(p => p.Property)
            .FirstOrDefault();

        return property != null;
    }

    public bool TryGetUserPropertyWhichUsesAsHome(int userId, out UserProperty property)
    {
        property = _dbContext.UserProperties.Where(r => r.UserId == userId && r.UsesAsHome)
            .Include(p => p.Property)
            .FirstOrDefault();

        return property != null;
    }

    public bool TryGetUserProperties(int userId, out List<UserProperty> properties)
    {
        properties = _dbContext.UserProperties.Where(r => r.UserId == userId)
            .Include(p => p.Property)
            .ToList();

        return properties.Count != 0;
    }

    public List<UserProperty> GetUserPropertyList(int userId)
    {
        return _dbContext.UserProperties.Where(r => r.UserId == userId)
            .Include(p => p.Property)
            .ToList();
    }

    public UserProperty AddNew(UserProperty property)
    {
        var entity = _dbContext.UserProperties.Add(property);
        _dbContext.SaveChanges();

        return entity.Entity;
    }

    public void Update(UserProperty property)
    {
        _dbContext.UserProperties.Update(property);
        _dbContext.SaveChanges();
    }

    public void Remove(UserProperty property)
    {
        _dbContext.UserProperties.Remove(property);
        _dbContext.SaveChanges();
    }
}
