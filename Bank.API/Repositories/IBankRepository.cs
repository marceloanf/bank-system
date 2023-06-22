using Bank.API.Models;

namespace Bank.API.Repositories
{
    public interface IBankRepository
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int userId);
        IEnumerable<Account> GetAccountsByUserId(int userId);
        Account GetAccountById(int accountId);
        void CreateUser(User user);
        void DeleteUser(int userId);
        void CreateAccount(int userId, Account account);
        void DeleteAccount(int accountId);
        void UpdateAccount(Account account);
        void SaveChanges();
    }
}