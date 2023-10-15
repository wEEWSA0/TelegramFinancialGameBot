using Microsoft.EntityFrameworkCore;

using TelegramFinancialGameBot.Data;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Model;

internal class RoomRepository
{
    private DatabaseContext _dbContext;

    public RoomRepository()
    {
        _dbContext = DbContextFactory.CreateDbContext();
    }

    public Room GetByIdOrThrow(int roomId)
    {
        Room room = _dbContext.Rooms.Where(r => r.Id == roomId)
            .Include(r => r.VictoryConditions).FirstOrDefault();

        if (room != null)
        {
            return room;
        }
        else
        {
            throw new KeyNotFoundException("Value not found");
        }
    }

    public bool TryGet(int id, out Room room)
    {
        room = _dbContext.Rooms.Where(r => r.Id == id).Include(r => r.VictoryConditions).FirstOrDefault();

        return room != null;
    }

    public bool TryGet(string name, out Room room)
    {
        room = _dbContext.Rooms.Where(r => r.Name == name)
            .Include(r => r.VictoryConditions).FirstOrDefault(); 

        return room != null;
    }

    public Room AddNew(Room room)
    {
        var newRoom = _dbContext.Rooms.Add(room);
        _dbContext.SaveChanges();

        return newRoom.Entity;
    }

    public void Update(Room room)
    {
        _dbContext.Rooms.Update(room);
        _dbContext.SaveChanges();
    }

    public void Remove(Room room)
    {
        _dbContext.Rooms.Remove(room);
        _dbContext.SaveChanges();
    }

    public bool TryRemoveByName(string roomName)
    {
        if (!TryGet(roomName, out Room room))
        {
            return false;
        }

        _dbContext.Rooms.Remove(room);
        _dbContext.SaveChanges();

        return true;
    }
}
