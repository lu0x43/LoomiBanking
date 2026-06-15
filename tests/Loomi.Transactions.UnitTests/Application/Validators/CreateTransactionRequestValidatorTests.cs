using FluentAssertions;
using Loomi.Transactions.Application.DTOs;
using Loomi.Transactions.Application.Validators;
using Xunit;

namespace Loomi.Transactions.UnitTests.Application.Validators;

public class CreateTransactionRequestValidatorTests
{
    private readonly CreateTransactionRequestValidator _validator;

    public CreateTransactionRequestValidatorTests()
    {
        _validator = new CreateTransactionRequestValidator();
    }

    [Fact]
    public void Validate_WithValidRequest_ShouldNotHaveErrors()
    {
        //Cria transferência perfeita
        var request = new CreateTransactionRequest(Guid.NewGuid(), Guid.NewGuid(), 150.00m);

        //Ac: Submete ao validador
        var result = _validator.Validate(request);

        //Validação usando FluentAssertion
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithSameOriginAndDestination_ShouldHaveValidationError()
    {
        //Tenta transferir dinheiro para a própria conta
        var sameClientId = Guid.NewGuid();
        var request = new CreateTransactionRequest(sameClientId, sameClientId, 100.00m);

        var result = _validator.Validate(request);

        //Esperado: O sistema DEVE barrar e apontar o erro no campo FromClientId
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FromClientId");
    }

    [Fact]
    public void Validate_WithZeroAmount_ShouldHaveValidationError()
    {
        //Tentativa de transferir R$ 0,00
        var request = new CreateTransactionRequest(Guid.NewGuid(), Guid.NewGuid(), 0m);
        
        var result = _validator.Validate(request);

        //O sistema DEVE barrar e apontar o erro no campo Amount
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Amount");
    }
}