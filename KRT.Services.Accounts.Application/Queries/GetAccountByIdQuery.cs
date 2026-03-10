using KRT.Services.Accounts.Application.MediatR;
using KRT.Services.Accounts.Application.ViewModels;

namespace KRT.Services.Accounts.Application.Queries;

public record GetAccountByIdQuery : IRequest<ResultViewModel<AccountViewModel>>
{
    public GetAccountByIdQuery(int id)
    {
        Id = id;
    }

    /// <summary>
    /// Id da Conta.
    /// </summary>
    public int Id { get; private set; }
}
