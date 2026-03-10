using KRT.Services.Accounts.Application.Caching;
using KRT.Services.Accounts.Application.MediatR;
using KRT.Services.Accounts.Application.ViewModels;
using KRT.Services.Accounts.Core.Entities;
using KRT.Services.Accounts.Core.Repositories;
using KRT.Services.Accounts.Infrastructure.CacheStorage;
using KRT.Services.Accounts.Infrastructure.MessageBus;

namespace KRT.Services.Accounts.Application.Commands.Handlers;

public class AddAccountHandler : IRequestHandler<AddAccountCommand, ResultViewModel<int>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICacheService _cacheService;

    public AddAccountHandler(IAccountRepository accountRepository, IEventProcessor eventProcessor, ICacheService cacheService)
    {
        _accountRepository = accountRepository;
        _eventProcessor = eventProcessor;
        _cacheService = cacheService;
    }

    public async Task<ResultViewModel<int>> HandleAsync(AddAccountCommand request, CancellationToken cancellationToken)
    {
        var accountExists = await _accountRepository.ExistsByCpfAsync(request.Cpf);

        if (accountExists)
            return ResultViewModel<int>.Error("Já existe uma conta cadastrada para o CPF informado.", 400);

        var account = Account.Create(request.HolderName, request.Cpf);

        await _accountRepository.AddAsync(account);

        await _eventProcessor.ProcessAsync(account.Events);

        await _cacheService.RemoveAsync(CacheKeys.AccountsList);

        return ResultViewModel<int>.Success(account.Id, 201);
    }
}
