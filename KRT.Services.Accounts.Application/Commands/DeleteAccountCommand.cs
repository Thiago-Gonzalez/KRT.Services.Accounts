using KRT.Services.Accounts.Application.MediatR;
using KRT.Services.Accounts.Application.ViewModels;

namespace KRT.Services.Accounts.Application.Commands;

public record DeleteAccountCommand : IRequest<ResultViewModel<Unit>>
{
    /// <summary>
    /// Id da Conta.
    /// </summary>
    public int Id { get; set; }
}
