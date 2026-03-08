using KRT.Services.Accounts.Core.Entities;

namespace KRT.Services.Accounts.Core.Repositories;

public interface IAccountRepository
{
    /// <summary>
    /// Verifica se já existe uma Conta cadastrada com o mesmo CPF.
    /// </summary>
    /// <param name="cpf">CPF do titular.</param>
    /// <returns>True se já existir uma Conta com o mesmo CPF, senão False.</returns>
    Task<bool> ExistsByCpfAsync(string cpf);

    /// <summary>
    /// Adiciona uma Conta.
    /// </summary>
    /// <param name="account">Conta a ser adicionada.</param>
    Task AddAsync(Account account);

    /// <summary>
    /// Obtém uma Conta por Id.
    /// </summary>
    /// <param name="id">Id.</param>
    Task<Account?> GetByIdAsync(int id);

    /// <summary>
    /// Obtém todas as Contas cadastradas.
    /// </summary>
    Task<List<Account>> GetAllAsync();

    /// <summary>
    /// Persiste as alterações.
    /// </summary>
    Task SaveChangesAsync();
}
