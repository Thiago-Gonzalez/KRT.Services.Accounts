using KRT.Services.Accounts.Core.Entities;
using KRT.Services.Accounts.Core.Enums;

namespace KRT.Services.Accounts.Application.ViewModels;

public record AccountViewModel
{
    public AccountViewModel(int id, string holderName, string cpf, AccountStatusEnum status)
    {
        Id = id;
        HolderName = holderName;
        Cpf = cpf;
        Status = status;
    }

    /// <summary>
    /// Id.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Nome do titular.
    /// </summary>
    public string HolderName { get; private set; } = string.Empty;

    /// <summary>
    /// CPF do titular.
    /// </summary>
    public string Cpf { get; private set; } = string.Empty;

    /// <summary>
    /// Status da Conta.
    /// </summary>
    public AccountStatusEnum Status { get; private set; }

    public static AccountViewModel FromEntity(Account account)
        => new(account.Id, account.HolderName, account.Cpf.Value, account.Status);
}
