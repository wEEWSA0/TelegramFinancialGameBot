using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramFinancialGameBot.Data;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Model;

internal class AccountRepository
{
    private DatabaseContext _dbContext;

    public AccountRepository()
    {
        _dbContext = DbContextFactory.CreateDbContext();
    }

    public List<Account> GetAll()
    {
        return _dbContext.Accounts.ToList();
    }

    public bool TryGet(long chatId, out Account user)
    {
        user = _dbContext.Accounts.ToList().FirstOrDefault((u) => u.ChatId == chatId);
        
        return user != null;
    }

    public void AddNew(Account user)
    {
        _dbContext.Accounts.Add(user);
        _dbContext.SaveChanges();
    }
}
