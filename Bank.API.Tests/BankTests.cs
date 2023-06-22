using Bank.API.Controllers;
using Bank.API.Models;
using Bank.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Bank.API.Tests;

public class BankTests
{
    private readonly UsersController _usersController;
    private readonly AccountsController _accountsController;
    private readonly Mock<IBankRepository> _repositoryMock;

    public BankTests()
    {
        _repositoryMock = new Mock<IBankRepository>();
        _usersController = new UsersController(_repositoryMock.Object);
        _accountsController = new AccountsController(_repositoryMock.Object);
    }

    [Fact]
    public void CreateUser_ValidUser_ReturnsCreatedResult()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Marcelo N" };

        // Act
        var result = _usersController.CreateUser(user);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(UsersController.GetUser), createdResult.ActionName);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public void DeleteUser_ExistingUser_ReturnsNoContentResult()
    {
        // Arrange
        var userId = 1;
        _repositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(new User { Id = userId });

        // Act
        var result = _usersController.DeleteUser(userId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void GetAccount_ExistingAccount_ReturnsOkResult()
    {
        // Arrange
        var accountId = 1;
        _repositoryMock.Setup(repo => repo.GetAccountById(accountId)).Returns(new Account { Id = accountId });

        // Act
        var result = _accountsController.GetAccount(accountId);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }
        
    [Fact]
    public void CreateAccount_ValidAccount_ReturnsCreatedAccount()
    {
        // Arrange
        var userId = 1;
        var newAccount = new Account { Id = 0, Balance = 1000, UserId = userId };

        _repositoryMock.Setup(repo => repo.GetAccountById(newAccount.Id)).Returns((Account)null);
        _repositoryMock.Setup(repo => repo.GetUsers()).Returns(new List<User> { new User { Id = userId, Name = "John", Accounts = new List<Account>() } });
        _repositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(new User { Id = userId, Name = "John", Accounts = new List<Account>() });
        _repositoryMock.Setup(repo => repo.CreateAccount(userId, newAccount));
        _repositoryMock.Setup(repo => repo.SaveChanges());

        // Act
        var result = _accountsController.CreateAccount(userId, newAccount) as ActionResult<Account>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CreatedAtActionResult>(result.Result);

        var createdResult = result.Result as CreatedAtActionResult;
        Assert.Equal(nameof(AccountsController.GetAccount), createdResult.ActionName);
        Assert.Equal(newAccount.Id, createdResult.RouteValues["accountId"]);
        Assert.Equivalent(newAccount, createdResult.Value);
    }


    [Fact]
    public void Deposit_ValidDepositAmount_UpdatesAccountBalance()
    {
        // Arrange
        var accountId = 1;
        var account = new Account { Id = accountId, Balance = 5000 };
        var depositAmount = 2000;

        _repositoryMock.Setup(repo => repo.GetAccountById(accountId)).Returns(account);
        _repositoryMock.Setup(repo => repo.UpdateAccount(account));
        _repositoryMock.Setup(repo => repo.SaveChanges());

        // Act
        var result = _accountsController.Deposit(accountId, depositAmount) as ActionResult<Account>;

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(account, okResult.Value);
        Assert.Equal(account.Balance, 7000);
    }

    [Fact]
    public void Withdraw_ValidWithdrawalAmount_UpdatesAccountBalance()
    {
        // Arrange
        var accountId = 1;
        var account = new Account { Id = accountId, Balance = 5000 };
        var withdrawalAmount = 2000;

        _repositoryMock.Setup(repo => repo.GetAccountById(accountId)).Returns(account);
        _repositoryMock.Setup(repo => repo.UpdateAccount(account));
        _repositoryMock.Setup(repo => repo.SaveChanges());

        // Act
        var result = _accountsController.Withdraw(accountId, withdrawalAmount) as ActionResult<Account>;

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(account, okResult.Value);
        Assert.Equal(account.Balance, 3000);
    }

    [Fact]
    public void Withdraw_ValidAmount_ReturnsOkResult()
    {
        // Arrange
        var accountId = 1;
        var account = new Account { Id = accountId, Balance = 1000 };
        _repositoryMock.Setup(repo => repo.GetAccountById(accountId)).Returns(account);

        // Act
        var result = _accountsController.Withdraw(accountId, 500);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

}
