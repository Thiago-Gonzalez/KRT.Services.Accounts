using KRT.Services.Accounts.Core.Exceptions;
using System.Text.RegularExpressions;

namespace KRT.Services.Accounts.Core.ValueObjects;

public sealed class Cpf
{
    public string Value { get; } = string.Empty;

    private Cpf(string cpf) => Value = cpf;

    /// <summary>
    /// Cria um CPF removendo formatações e verificando se o CPF é válido.
    /// </summary>
    /// <param name="value">CPF.</param>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public static Cpf Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new BusinessRuleValidationException("CPF é obrigatório.");

        var cpf = RemoveFormatting(value);

        if (!IsValid(cpf))
            throw new BusinessRuleValidationException("CPF inválido.");

        return new Cpf(cpf);
    }

    /// <summary>
    /// Remove formatações do CPF.
    /// </summary>
    /// <param name="cpf">CPF</param>
    /// <returns>CPF sem formatação.</returns>
    private static string RemoveFormatting(string cpf) => cpf.Replace(".", "").Replace("-", "");

    /// <summary>
    /// Verifica se um CPF é válido.
    /// </summary>
    /// <param name="cpf">CPF.</param>
    /// <returns>True se o CPF for válido, senão False.</returns>
    private static bool IsValid(string cpf)
    {
        var regex = new Regex(@"^\d{3}\d{3}\d{3}\d{2}$");

        if (!regex.IsMatch(cpf))
            return false;

        if (cpf.All(d => d == cpf[0]))
            return false;

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
        }

        int rest = sum % 11;
        int firstVerifyingDigit = (rest < 2) ? 0 : (11 - rest);

        if (firstVerifyingDigit != int.Parse(cpf[9].ToString()))
            return false;

        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
        }

        rest = sum % 11;
        int secondVerifyingDigit = (rest < 2) ? 0 : (11 - rest);

        if (secondVerifyingDigit != int.Parse(cpf[10].ToString()))
            return false;

        return true;

    }

    public override string ToString() => Value;
}
