using Microsoft.EntityFrameworkCore;

using TelegramFinancialGameBot.Data;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Model;

internal class UserRepository
{
    private DatabaseContext _dbContext;

    public UserRepository()
    {
        _dbContext = DbContextFactory.CreateDbContext();
    }

    public User GetByIdOrThrow(int userId)
    {
        User userInRoom = _dbContext.Users.Where(r => r.Id == userId)
            .Include(r => r.Account)
            .Include(r => r.Room.VictoryConditions)
            .Include(r => r.Dream).FirstOrDefault();

        if (userInRoom != null)
        {
            return userInRoom;
        }
        else
        {
            throw new KeyNotFoundException("Value not found");
        }
    }

    public bool TryGetById(int userId, out User userInRoom)
    {
        userInRoom = _dbContext.Users.Where(r => r.Id == userId)
            .Include(r => r.Account)
            .Include(r => r.Room.VictoryConditions)
            .Include(r => r.Dream).FirstOrDefault();

        return userInRoom != null;
    }

    public bool TryGet(long userChatId, int roomId, out User userInRoom)
    {
        userInRoom = _dbContext.Users.Where(r => r.AccountChatId == userChatId && r.RoomId == roomId)
            .Include(r => r.Account)
            .Include(r => r.Room.VictoryConditions).FirstOrDefault();

        return userInRoom != null;
    }

    public bool TryGetWithDream(long userChatId, int roomId, out User userInRoom)
    {
        userInRoom = _dbContext.Users.Where(r => r.AccountChatId == userChatId && r.RoomId == roomId)
            .Include(r => r.Account)
            .Include(r => r.Room.VictoryConditions)
            .Include(r => r.Dream).FirstOrDefault();

        return userInRoom != null;
    }

    public bool TryGetWithDreamIfOnlyOneUserInRoomExist(long userChatId, out User userInRoom)
    {
        userInRoom = _dbContext.Users.Where(r => r.AccountChatId == userChatId)
            .Include(r => r.Account)
            .Include(r => r.Room.VictoryConditions)
            .Include(r => r.Dream).FirstOrDefault();

        return userInRoom != null;
    }

    public bool TryGetUserRooms(long userChatId, out Room[] rooms)
    {
        var userInRooms = _dbContext.Users.Where(r => r.AccountChatId == userChatId)
            .Include(r => r.Account)
            .Include(r => r.Room);
        
        if (userInRooms.Any())
        {
            rooms = userInRooms.Select(r => r.Room).ToArray();

            return true;
        }

        rooms = null;

        return false;
    }

    public bool TryGetUsersInRoom(long roomId, out List<User> users)
    {
        users = _dbContext.Users.Where(r => r.RoomId == roomId)
            .Include(r => r.Account)
            .Include(r => r.Room.VictoryConditions)
            .Include(r => r.Dream)
            .ToList();

        return users.Count > 0;
    }

    public bool TryGetUsersInRoomWithoutRoom(long roomId, out List<User> users)
    {
        users = _dbContext.Users.Where(r => r.RoomId == roomId)
            .Include(r => r.Account)
            .Include(r => r.Dream)
            .ToList();

        return users.Count > 0;
    }

    public User AddNew(User userInRoom)
    {
        var user = _dbContext.Users.Add(userInRoom);
        _dbContext.SaveChanges();

        return user.Entity;
    }

    public void Update(User userInRoom)
    {
        _dbContext.Users.Update(userInRoom);
        _dbContext.SaveChanges();
    }

    public void UpdateRange(User[] users)
    {
        _dbContext.Users.UpdateRange(users);
        _dbContext.SaveChanges();
    }

    public void Remove(User userInRoom)
    {
        _dbContext.Users.Remove(userInRoom);
        _dbContext.SaveChanges();
    }
}
