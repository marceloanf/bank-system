using MediatR;

namespace Bank.Application.Accounts.Commands.CreateAccount;

public class CreateAccountRequest : IRequest<CreateAccountResponse>
{
    public string Name { get; set; }
}