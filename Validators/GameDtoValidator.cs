using FluentValidation;
using GameStoreApi.DTOs;

namespace GameStoreApi.Validators;

public class CreateGameDtoValidator : AbstractValidator<CreateGameDto>
{
    public CreateGameDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Game name is required.")
            .MaximumLength(100).WithMessage("Game name cannot exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

        RuleFor(x => x.GenreId)
            .GreaterThan(0).WithMessage("Please select a valid Genre.");

        RuleFor(x => x.PublisherId)
            .GreaterThan(0).WithMessage("Please select a valid Publisher.");
    }
    
    public class UpdateGameDtoValidator : AbstractValidator<CreateGameDto>
    {
        public UpdateGameDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Game name is required.")
                .MaximumLength(100).WithMessage("Game name cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be 0 or greater.");

            RuleFor(x => x.GenreId)
                .GreaterThan(0).WithMessage("Please select a valid Genre.");

            RuleFor(x => x.PublisherId)
                .GreaterThan(0).WithMessage("Please select a valid Publisher.");
        }
    }
}