using FluentValidation;
using Loomi.Transactions.Application.DTOs;

namespace Loomi.Transactions.Application.Validators;

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.FromClientId)
            .NotEmpty().WithMessage("O ID do cliente de origem é obrigatório.");

        RuleFor(x => x.ToClientId)
            .NotEmpty().WithMessage("O ID do cliente de destino é obrigatório.");

        RuleFor(x => x.FromClientId)
            .NotEqual(x => x.ToClientId).WithMessage("A conta de origem e destino não podem ser as mesmas.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("O valor da transferência deve ser maior que zero.");
    }
}