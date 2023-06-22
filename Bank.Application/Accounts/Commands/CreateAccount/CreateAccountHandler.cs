using MediatR;

namespace Bank.Application.Accounts.Commands.CreateAccount;

public sealed class CreateAccountHandler : IRequestHandler<CreateAccountRequest, CreateAccountResponse>
{
    public Task<CreateAccountResponse> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}