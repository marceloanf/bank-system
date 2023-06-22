using Bank.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank.API.Data
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Seed initial data
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "User 1" }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, Balance = 1000, UserId = 1 }
            );
        }
    }
}
