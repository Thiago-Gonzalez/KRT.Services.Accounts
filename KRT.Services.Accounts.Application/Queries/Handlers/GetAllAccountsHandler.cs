using KRT.Services.Accounts.Application.Caching;
using KRT.Services.Accounts.Application.MediatR;
using KRT.Services.Accounts.Application.ViewModels;
using KRT.Services.Accounts.Core.Repositories;
using KRT.Services.Accounts.Infrastructure.CacheStorage;

namespace KRT.Services.Accounts.Application.Queries.Handlers;

public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, ResultViewModel<List<AccountViewModel>>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICacheService _cacheService;

    public GetAllAccountsHandler(IAccountRepository accountRepository, ICacheService cacheService)
    {
        _accountRepository = accountRepository;
        _cacheService = cacheService;
    }

    public async Task<ResultViewModel<List<AccountViewModel>>> HandleAsync(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var cachedAccounts = await _cacheService.GetAsync<List<AccountViewModel>>(CacheKeys.AccountsList);

        if (cachedAccounts is not null)
            return ResultViewModel<List<AccountViewModel>>.Success(cachedAccounts);

        var accounts = await _accountRepository.GetAllAsync();

        var viewModel = accounts.Select(AccountViewModel.FromEntity).ToList();

        await _cacheService.SetAsync<List<AccountViewModel>>(CacheKeys.AccountsList, viewModel);

        return ResultViewModel<List<AccountViewModel>>.Success(viewModel);
    }
}
