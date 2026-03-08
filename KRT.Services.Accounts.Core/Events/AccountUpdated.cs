namespace KRT.Services.Accounts.Core.Events;

public class AccountUpdated : IDomainEvent
{
    public AccountUpdated(int id, string holderName, string cpf)
    {
        Id = id;
        HolderName = holderName;
        Cpf = cpf;
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
}
