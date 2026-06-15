using FluentValidation;
using Loomi.Clients.Application.DTOs;

namespace Loomi.Clients.Application.Validators;

public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("O nome completo é obrigatório.")
            .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.");

        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("O documento (CPF/CNPJ) é obrigatório.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O formato do e-mail é inválido.");

        RuleFor(x => x.BankCode)
            .NotEmpty().WithMessage("O código do banco é obrigatório.");

        RuleFor(x => x.Branch)
            .NotEmpty().WithMessage("A agência é obrigatória.");

        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("O número da conta é obrigatório.");
    }
}