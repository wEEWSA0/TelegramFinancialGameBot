using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TelegramFinancialGameBot.Data;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Model;

internal class StaffRepository
{
    private DatabaseContext _dbContext;

    public StaffRepository()
    {
        _dbContext = DbContextFactory.CreateDbContext();
    }

    public bool TryGetStaff(string staffId, out Staff staff)
    {
        staff = _dbContext.Staffs.Where(r => r.Id == staffId)
            .FirstOrDefault();

        return staff != null;
    }

    public bool TryGetManagerStaff(string staffId, out ManagerStaff staff)
    {
        staff = _dbContext.ManagerStaffs.Where(r => r.Id == staffId)
            .FirstOrDefault();

        return staff != null;
    }

    public bool TryGetRegionalDirector(string id, out RegionalDirector director)
    {
        director = _dbContext.RegionalDirectors.Where(r => r.Id == id)
            .FirstOrDefault();

        return director != null;
    }

    public bool TryGetGeneralDirector(string id, out GeneralDirector director)
    {
        director = _dbContext.GeneralDirectors.Where(r => r.Id == id)
            .FirstOrDefault();

        return director != null;
    }

    public bool TryGetFinancialDirector(string id, out FinancialDirector director)
    {
        director = _dbContext.FinancialDirectors.Where(r => r.Id == id)
            .FirstOrDefault();

        return director != null;
    }

    public bool TryGetBuisnessManagerStaff(int buisnessId, string staffId, out BuisnessManagerStaff staff)
    {
        staff = _dbContext.BuisnessManagerStaffs.Where(r => r.UserBuisnessId == buisnessId && r.ManagerStaffId == staffId)
            .Include(r => r.ManagerStaff)
            .FirstOrDefault();

        return staff != null;
    }

    public bool TryGetBuisnessRegionalDirector(int buisnessId, string staffId, out BuisnessRegionalDirector staff)
    {
        staff = _dbContext.BuisnessRegionalDirectors.Where(r => r.UserBuisnessId == buisnessId && r.RegionalDirectorId == staffId)
            .Include(r => r.RegionalDirector)
            .FirstOrDefault();

        return staff != null;
    }

    public bool TryGetUserStaff(int userId, string staffId, out UserStaff staff)
    {
        staff = _dbContext.UserStaffs.Where(r => r.UserId == userId && r.StaffId == staffId)
            .Include(r => r.Staff)
            .FirstOrDefault();

        return staff != null;
    }

    public List<BuisnessManagerStaff> GetBuisnessManagerStaffList(int buisnessId)
    {
        return _dbContext.BuisnessManagerStaffs.Where(w => w.UserBuisnessId == buisnessId)
            .Include(b => b.ManagerStaff)
            .ToList();
    }

    public List<ManagerStaff> GetUserManagerStaffList(int userId)
    {
        return _dbContext.BuisnessManagerStaffs.Where(w => w.UserBuisness.UserId == userId)
            .Select(b => b.ManagerStaff)
            .ToList();
    }

    public List<UserStaff> GetUserStaffList(int userId)
    {
        return _dbContext.UserStaffs.Where(w => w.UserId == userId)
            .Include(b => b.Staff)
            .ToList();
    }

    public List<Staff> GetStaffList(int userId)
    {
        return _dbContext.UserStaffs.Where(w => w.UserId == userId)
            .Select(b => b.Staff)
            .ToList();
    }

    public List<BuisnessRegionalDirector> GetBuisnessRegionalDirectorList(int buisnessId)
    {
        return _dbContext.BuisnessRegionalDirectors.Where(w => w.UserBuisnessId == buisnessId)
            .Include(b => b.RegionalDirector)
            .ToList();
    }

    public List<RegionalDirector> GetUserRegionalDirectorList(int userId)
    {
        return _dbContext.BuisnessRegionalDirectors.Where(w => w.UserBuisness.UserId == userId)
            .Select(b => b.RegionalDirector)
            .ToList();
    }

    public List<FinancialDirector> GetUserFinancialDirectorList(int userId)
    {
        return _dbContext.UserBuisnesses.Where(w => w.UserId == userId && w.FinancialDirectorId != null)
            .Select(a => a.FinancialDirector)
            .ToList();
    }

    public List<GeneralDirector> GetUserGeneralDirectorList(int userId)
    {
        return _dbContext.UserBuisnesses.Where(w => w.UserId == userId && w.GeneralDirectorId != null)
            .Select(a => a.GeneralDirector)
            .ToList();
    }

    //public List<> GetBuisnessRegionalDirectorList(int userId)
    //{
    //    return _dbContext.BuisnessRegionalDirectors.Where(w => w.UserBuisnessId == userId)
    //        .Include(entity => entity.RegionalDirector)
    //        .ToList();
    //}

    public UserStaff AddNew(UserStaff userStaff)
    {
        var entity = _dbContext.UserStaffs.Add(userStaff);
        _dbContext.SaveChanges();

        return entity.Entity;
    }

    public void AddNew(BuisnessManagerStaff userStaff)
    {
        _dbContext.BuisnessManagerStaffs.Add(userStaff);
        _dbContext.SaveChanges();
    }

    public void AddNew(BuisnessRegionalDirector userStaff)
    {
        _dbContext.BuisnessRegionalDirectors.Add(userStaff);
        _dbContext.SaveChanges();
    }

    public void AddNew(FinancialDirector userStaff, int buisnessId)
    {
        var buisness = _dbContext.UserBuisnesses.Where(w => w.Id == buisnessId).First();

        buisness.FinancialDirectorId = userStaff.Id;

        _dbContext.UserBuisnesses.Update(buisness);

        _dbContext.SaveChanges();
    }

    public void AddNew(GeneralDirector userStaff, int buisnessId)
    {
        var buisness = _dbContext.UserBuisnesses.Where(w => w.Id == buisnessId).First();

        buisness.GeneralDirectorId = userStaff.Id;

        _dbContext.UserBuisnesses.Update(buisness);

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

    public void Remove(UserStaff staff)
    {
        _dbContext.UserStaffs.Remove(staff);
        _dbContext.SaveChanges();
    }
}
