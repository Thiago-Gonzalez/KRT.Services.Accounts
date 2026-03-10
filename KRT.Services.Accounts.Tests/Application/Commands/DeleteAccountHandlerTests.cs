using KRT.Services.Accounts.Application.Caching;
using KRT.Services.Accounts.Application.Commands;
using KRT.Services.Accounts.Application.Commands.Handlers;
using KRT.Services.Accounts.Core.Entities;
using KRT.Services.Accounts.Core.Events;
using KRT.Services.Accounts.Core.Repositories;
using KRT.Services.Accounts.Infrastructure.CacheStorage;
using KRT.Services.Accounts.Infrastructure.MessageBus;
using NSubstitute;

namespace KRT.Services.Accounts.Tests.Application.Commands;

public class DeleteAccountHandlerTests
{
    private readonly IAccountRepository _repository;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICacheService _cacheService;
    private readonly DeleteAccountHandler _handler;

    public DeleteAccountHandlerTests()
    {
        _repository = Substitute.For<IAccountRepository>();
        _eventProcessor = Substitute.For<IEventProcessor>();
        _cacheService = Substitute.For<ICacheService>();

        _handler = new DeleteAccountHandler(_repository, _eventProcessor, _cacheService);
    }

    [Fact]
    public async Task ExistingAccountId_Executed_ReturnSuccess()
    {
        // Arrange
        const int accountId = 1;

        var account = Account.Create("John Doe", "52998224725");

        _repository.GetByIdAsync(accountId)
            .Returns(account);

        var command = new DeleteAccountCommand()
        {
            Id = accountId
        };

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        await _repository.Received(1)
            .SaveChangesAsync();

        await _eventProcessor.Received(1)
            .ProcessAsync(Arg.Any<IEnumerable<AccountDeleted>>());

        await _cacheService.Received(1)
            .RemoveAsync(CacheKeys.AccountById(command.Id));
        await _cacheService.Received(1)
            .RemoveAsync(CacheKeys.AccountsList);
    }

    [Fact]
    public async Task NonExistingAccountId_Executed_ReturnNotFound()
    {
        // Arrange
        var command = new DeleteAccountCommand()
        {
            Id = 123
        };

        _repository.GetByIdAsync(command.Id)
            .Returns((Account?)null);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Conta não encontrada.", result.Message);

        await _repository.DidNotReceive()
            .SaveChangesAsync();

        await _eventProcessor.DidNotReceive()
        .ProcessAsync(Arg.Any<IEnumerable<AccountDeleted>>());

        await _cacheService.DidNotReceive()
            .RemoveAsync(CacheKeys.AccountById(command.Id));
        await _cacheService.DidNotReceive()
            .RemoveAsync(CacheKeys.AccountsList);
    }
}
