using KRT.Services.Accounts.Application.Caching;
using KRT.Services.Accounts.Application.MediatR;
using KRT.Services.Accounts.Application.ViewModels;
using KRT.Services.Accounts.Core.Exceptions;
using KRT.Services.Accounts.Core.Repositories;
using KRT.Services.Accounts.Infrastructure.CacheStorage;
using KRT.Services.Accounts.Infrastructure.MessageBus;

namespace KRT.Services.Accounts.Application.Commands.Handlers;

public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, ResultViewModel<Unit>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICacheService _cacheService;

    public DeleteAccountHandler(
        IAccountRepository accountRepository,
        IEventProcessor eventProcessor,
        ICacheService cacheService)
    {
        _accountRepository = accountRepository;
        _eventProcessor = eventProcessor;
        _cacheService = cacheService;
    }

    public async Task<ResultViewModel<Unit>> HandleAsync(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.Id);

        if (account is null)
            return ResultViewModel<Unit>.Error("Conta não encontrada.", 404);

        account.Deactivate();

        await _accountRepository.SaveChangesAsync();

        await _eventProcessor.ProcessAsync(account.Events);

        await _cacheService.RemoveAsync(CacheKeys.AccountById(request.Id));
        await _cacheService.RemoveAsync(CacheKeys.AccountsList);

        return ResultViewModel<Unit>.Success(Unit.Value);
    }
}
