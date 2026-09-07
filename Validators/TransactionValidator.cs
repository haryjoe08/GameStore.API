using FluentValidation;
using GameStoreApi.DTOs;

namespace GameStoreApi.Validators;

public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionDto>
{
    public CreateTransactionDtoValidator()
    {
        RuleFor(x => x.GameId)
            .GreaterThan(0).WithMessage("Please select a valid Game.");
    }
}