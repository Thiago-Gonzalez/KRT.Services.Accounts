using KRT.Services.Accounts.Application.Caching;
using KRT.Services.Accounts.Application.Queries;
using KRT.Services.Accounts.Application.Queries.Handlers;
using KRT.Services.Accounts.Application.ViewModels;
using KRT.Services.Accounts.Core.Entities;
using KRT.Services.Accounts.Core.Repositories;
using KRT.Services.Accounts.Infrastructure.CacheStorage;
using NSubstitute;

namespace KRT.Services.Accounts.Tests.Application.Queries;

public class GetAccountByIdHandlerTests
{
    private readonly GetAccountByIdHandler _handler;
    private readonly IAccountRepository _repository;
    private readonly ICacheService _cacheService;

    public GetAccountByIdHandlerTests()
    {
        _repository = Substitute.For<IAccountRepository>();
        _cacheService = Substitute.For<ICacheService>();

        _handler = new GetAccountByIdHandler(_repository, _cacheService);
    }

    [Fact]
    public async Task ExistingAccountId_Executed_ReturnsOkAndAccount()
    {
        // Arrange
        const int accountId = 1;

        var account = Account.Create("John Doe", "52998224725");

        _repository.GetByIdAsync(accountId)
            .Returns(account);

        var query = new GetAccountByIdQuery(accountId);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.StatusCode);

        var accountViewModel = AccountViewModel.FromEntity(account);

        Assert.Equal(accountViewModel, result.Value);

        await _repository.Received(1)
            .GetByIdAsync(accountId);

        await _cacheService.Received(1)
            .GetAsync<AccountViewModel>(CacheKeys.AccountById(accountId));
        await _cacheService.Received(1)
            .SetAsync(CacheKeys.AccountById(accountId), accountViewModel);
    }
}
