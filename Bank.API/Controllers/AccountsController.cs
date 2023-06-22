using Bank.API.Models;
using Bank.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bank.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IBankRepository _repository;

    public AccountsController(IBankRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("users/{userId}")]
    public ActionResult<IEnumerable<Account>> GetAccountsByUserId(int userId)
    {
        var accounts = _repository.GetAccountsByUserId(userId);
        return Ok(accounts);
    }

    [HttpGet("{accountId}")]
    public ActionResult<Account> GetAccount(int accountId)
    {
        var account = _repository.GetAccountById(accountId);
        if (account == null)
        {
            return NotFound();
        }

        return Ok(account);
    }

    [HttpPost("users/{userId}")]
    public ActionResult<Account> CreateAccount(int userId, [FromBody] Account account)
    {
        var user = _repository.GetUserById(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }
        
        // Check if deposit amount exceeds the limit
        if (account.Balance > 10000)
        {
            return BadRequest("Deposit amount exceeds the maximum limit");
        }

        account.UserId = userId;

        // Create a new account instance and add it to the user's accounts collection
        var newAccount = new Account
        {
            Balance = account.Balance,
            UserId = userId
        };

        user.Accounts.Add(newAccount);

        _repository.SaveChanges();

        return CreatedAtAction(nameof(GetAccount), new { accountId = newAccount.Id }, newAccount);
    }




    [HttpDelete("{accountId}")]
    public ActionResult DeleteAccount(int accountId)
    {
        var account = _repository.GetAccountById(accountId);
        if (account == null)
        {
            return NotFound();
        }

        _repository.DeleteAccount(accountId);
        _repository.SaveChanges();

        return NoContent();
    }

    [HttpPost("{accountId}/deposit")]
    public ActionResult<Account> Deposit(int accountId, decimal amount)
    {
        var account = _repository.GetAccountById(accountId);
        if (account == null)
        {
            return NotFound();
        }

        // Check if deposit amount exceeds the limit
        if (amount > 10000)
        {
            return BadRequest("Deposit amount exceeds the maximum limit");
        }

        account.Balance += amount;
        _repository.UpdateAccount(account);
        _repository.SaveChanges();

        return Ok(account);
    }


    [HttpPost("{accountId}/withdraw")]
    public ActionResult<Account> Withdraw(int accountId, decimal amount)
    {
        var account = _repository.GetAccountById(accountId);
        if (account == null)
        {
            return NotFound();
        }

        if (account.Balance - amount < 100)
        {
            return BadRequest("Account balance cannot be less than $100");
        }

        decimal maxWithdrawalAmount = account.Balance * 0.9m;
        if (amount > maxWithdrawalAmount)
        {
            return BadRequest("Withdrawal amount exceeds the maximum limit");
        }

        account.Balance -= amount;
        _repository.UpdateAccount(account);
        _repository.SaveChanges();

        return Ok(account);
    }

}
