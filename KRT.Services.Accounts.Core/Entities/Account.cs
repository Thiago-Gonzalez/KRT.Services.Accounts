using KRT.Services.Accounts.Core.Enums;
using KRT.Services.Accounts.Core.Events;
using KRT.Services.Accounts.Core.Exceptions;
using KRT.Services.Accounts.Core.ValueObjects;

namespace KRT.Services.Accounts.Core.Entities;

public class Account : AggregateRoot
{
    /// <summary>
    /// Construtor default para configuração do EF Core.
    /// </summary>
    private Account() { }

    private Account(string holderName, string cpf)
    {
        Validate(holderName);

        HolderName = holderName;
        Cpf = Cpf.Create(cpf);
        Status = AccountStatusEnum.Active;
    }

    /// <summary>
    /// Nome do titular.
    /// </summary>
    public string HolderName { get; private set; } = string.Empty;

    /// <summary>
    /// CPF do titular.
    /// </summary>
    public Cpf Cpf { get; private set; } = null!;

    /// <summary>
    /// Status da Conta.
    /// </summary>
    public AccountStatusEnum Status { get; private set; }

    /// <summary>
    /// Cria uma Conta e adiciona o Evento <see cref="AccountCreated"/>.
    /// </summary>
    /// <param name="holderName">Nome do titular.</param>
    /// <param name="cpf">CPF do titular.</param>
    public static Account Create(string holderName, string cpf)
    {
        var account = new Account(holderName, cpf);

        account.AddEvent(new AccountCreated(account.Id, account.HolderName, account.Cpf.Value));

        return account;
    }

    /// <summary>
    /// Atualiza os dados da Conta e adiciona o Evento <see cref="AccountUpdated"/>.
    /// </summary>
    /// <param name="holderName">Nome do titular.</param>
    /// <param name="cpf">CPF do titular.</param>
    public void Update(string holderName, string cpf)
    {
        if (Status == AccountStatusEnum.Inactive)
            throw new BusinessRuleValidationException("Contas inativas não podem ser atualizadas.");

        Validate(holderName);

        HolderName = holderName;
        Cpf = Cpf.Create(cpf);

        AddEvent(new AccountUpdated(Id, HolderName, Cpf.Value));
    }

    /// <summary>
    /// Desativa uma Conta, definindo o Status como Inativo, e adiciona o Evento <see cref="AccountDeleted"/>.
    /// </summary>
    public void Deactivate()
    {
        if (Status == AccountStatusEnum.Inactive)
            return;

        Status = AccountStatusEnum.Inactive;
        AddEvent(new AccountDeleted(Id));
    }

    /// <summary>
    /// Valida as propriedades da Conta.
    /// </summary>
    /// <param name="holderName">Nome do titular.</param>
    /// <exception cref="BusinessRuleValidationException"></exception>
    private static void Validate(string holderName)
    {
        if (string.IsNullOrWhiteSpace(holderName))
            throw new BusinessRuleValidationException("Nome do titular é obrigatório.");
    }
}
