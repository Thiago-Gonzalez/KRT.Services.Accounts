using KRT.Services.Accounts.Application.Caching;
using KRT.Services.Accounts.Application.Commands;
using KRT.Services.Accounts.Application.Commands.Handlers;
using KRT.Services.Accounts.Core.Entities;
using KRT.Services.Accounts.Core.Events;
using KRT.Services.Accounts.Core.Exceptions;
using KRT.Services.Accounts.Core.Repositories;
using KRT.Services.Accounts.Infrastructure.CacheStorage;
using KRT.Services.Accounts.Infrastructure.MessageBus;
using NSubstitute;

namespace KRT.Services.Accounts.Tests.Application.Commands;

public class AddAccountHandlerTests
{
    private readonly IAccountRepository _repository;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICacheService _cacheService;
    private readonly AddAccountHandler _handler;

    public AddAccountHandlerTests()
    {
        _repository = Substitute.For<IAccountRepository>();
        _eventProcessor = Substitute.For<IEventProcessor>();
        _cacheService = Substitute.For<ICacheService>();

        _handler = new AddAccountHandler(_repository, _eventProcessor, _cacheService);
    }

    [Fact]
    public async Task InputDataIsOk_Executed_ReturnCreated()
    {
        // Arrange
        var command = new AddAccountCommand()
        {
            HolderName = "John Doe",
            Cpf = "67021480032"
        };

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(201, result.StatusCode);

        await _repository.Received(1)
            .AddAsync(Arg.Any<Account>());

        await _eventProcessor.Received(1)
            .ProcessAsync(Arg.Any<IEnumerable<AccountCreated>>());

        await _cacheService.Received(1)
            .RemoveAsync(CacheKeys.AccountsList);
    }

    [Fact]
    public async Task CpfIsInvalid_Executed_ThrowBusinessRuleValidationException()
    {
        // Arrange
        var command = new AddAccountCommand()
        {
            HolderName = "John Doe",
            Cpf = "12312312312"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessRuleValidationException>(() =>
            _handler.HandleAsync(command, CancellationToken.None));

        Assert.Equal("CPF inválido.", exception.Message);

        await _repository.DidNotReceive()
            .AddAsync(Arg.Any<Account>());

        await _eventProcessor.DidNotReceive()
            .ProcessAsync(Arg.Any<IEnumerable<AccountCreated>>());

        await _cacheService.DidNotReceive()
            .RemoveAsync(CacheKeys.AccountsList);
    }
}
