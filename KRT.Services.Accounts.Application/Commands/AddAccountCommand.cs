using KRT.Services.Accounts.Application.MediatR;
using KRT.Services.Accounts.Application.ViewModels;

namespace KRT.Services.Accounts.Application.Commands;

public record AddAccountCommand : IRequest<ResultViewModel<int>>
{
    /// <summary>
    /// Nome do titular.
    /// </summary>
    public string HolderName { get; set; } = string.Empty;

    /// <summary>
    /// CPF do titular.
    /// </summary>
    public string Cpf { get; set; } = string.Empty;
}
