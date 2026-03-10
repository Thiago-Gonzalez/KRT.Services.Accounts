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

public class UpdateAccountHandlerTests
{
    private readonly IAccountRepository _repository;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICacheService _cacheService;
    private readonly UpdateAccountHandler _handler;

    public UpdateAccountHandlerTests()
    {
        _repository = Substitute.For<IAccountRepository>();
        _eventProcessor = Substitute.For<IEventProcessor>();
        _cacheService = Substitute.For<ICacheService>();

        _handler = new UpdateAccountHandler(_repository, _eventProcessor, _cacheService);
    }

    [Fact]
    public async Task InputDataIsOk_Executed_ReturnNoContent()
    {
        // Arrange
        const int accountId = 1;

        var account = Account.Create("John Doe", "52998224725");

        _repository.GetByIdAsync(accountId)
            .Returns(account);

        var command = new UpdateAccountCommand()
        {
            Id = accountId,
            HolderName = "John Doe",
            Cpf = "67021480032"
        };

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(204, result.StatusCode);

        await _repository.Received(1)
            .SaveChangesAsync();

        await _eventProcessor.Received(1)
            .ProcessAsync(Arg.Any<IEnumerable<AccountUpdated>>());

        await _cacheService.Received(1)
            .RemoveAsync(CacheKeys.AccountsList);
    }
}
