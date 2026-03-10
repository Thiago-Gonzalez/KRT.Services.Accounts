using KRT.Services.Accounts.Application.Caching;
using KRT.Services.Accounts.Application.MediatR;
using KRT.Services.Accounts.Application.ViewModels;
using KRT.Services.Accounts.Core.Repositories;
using KRT.Services.Accounts.Infrastructure.CacheStorage;

namespace KRT.Services.Accounts.Application.Queries.Handlers;

public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, ResultViewModel<AccountViewModel>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICacheService _cacheService;

    public GetAccountByIdHandler(IAccountRepository accountRepository, ICacheService cacheService)
    {
        _accountRepository = accountRepository;
        _cacheService = cacheService;
    }

    public async Task<ResultViewModel<AccountViewModel>> HandleAsync(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.AccountById(request.Id);
        var cachedAccount = await _cacheService.GetAsync<AccountViewModel>(cacheKey);

        if (cachedAccount is not null)
            return ResultViewModel<AccountViewModel>.Success(cachedAccount);

        var account = await _accountRepository.GetByIdAsync(request.Id);

        if (account is null)
            return ResultViewModel<AccountViewModel>.Error("Conta não encontrada", 404);

        var viewModel = AccountViewModel.FromEntity(account);

        await _cacheService.SetAsync(cacheKey, viewModel);

        return ResultViewModel<AccountViewModel>.Success(viewModel);
    }
}
