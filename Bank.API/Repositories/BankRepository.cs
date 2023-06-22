using Bank.API.Data;
using Bank.API.Models;

namespace Bank.API.Repositories;

public class BankRepository : IBankRepository
{
    private readonly BankDbContext _context;

    public BankRepository(BankDbContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetUsers()
    {
        return _context.Users.ToList();
    }

    public User GetUserById(int userId)
    {
        return _context.Users.FirstOrDefault(u => u.Id == userId);
    }

    public IEnumerable<Account> GetAccountsByUserId(int userId)
    {
        return _context.Accounts.Where(a => a.UserId == userId).ToList();
    }

    public Account GetAccountById(int accountId)
    {
        return _context.Accounts.FirstOrDefault(a => a.Id == accountId);
    }

    public void CreateUser(User user)
    {
        _context.Users.Add(user);
    }

    public void DeleteUser(int userId)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            _context.Users.Remove(user);
        }
    }

    public void CreateAccount(int userId, Account account)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            user.Accounts.Add(account);
        }
    }

    public void DeleteAccount(int accountId)
    {
        var account = _context.Accounts.FirstOrDefault(a => a.Id == accountId);
        if (account != null)
        {
            _context.Accounts.Remove(account);
        }
    }

    public void UpdateAccount(Account account)
    {
        _context.Accounts.Update(account);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
    
}
