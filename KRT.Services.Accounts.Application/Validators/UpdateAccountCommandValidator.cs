using FluentValidation;
using KRT.Services.Accounts.Application.Commands;

namespace KRT.Services.Accounts.Application.Validators;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(a => a.HolderName)
            .NotEmpty()
            .WithMessage("O Nome do titular é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O Nome do titular deve ter no máximo 100 caracteres.");

        RuleFor(a => a.Cpf)
            .NotEmpty()
            .WithMessage("O CPF do titular é obrigatório.")
            .Length(11)
            .WithMessage("O CPF deve conter 11 dígitos.")
            .Matches(@"^\d{11}$")
            .WithMessage("O CPF deve conter apenas números.");
    }
}
