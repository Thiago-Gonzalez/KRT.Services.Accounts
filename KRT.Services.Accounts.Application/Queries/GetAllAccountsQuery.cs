using KRT.Services.Accounts.Application.MediatR;
using KRT.Services.Accounts.Application.ViewModels;

namespace KRT.Services.Accounts.Application.Queries;

public record GetAllAccountsQuery : IRequest<ResultViewModel<List<AccountViewModel>>>
{
}
