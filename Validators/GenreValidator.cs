using FluentValidation;
using GameStoreApi.DTOs;

namespace GameStoreApi.Validators;

public class CreateGenreDtoValidator : AbstractValidator<CreateGenreDto>
{
    public CreateGenreDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Genre name is required.")
            .MaximumLength(50).WithMessage("Genre name cannot exceed 50 characters.");
    }
    
    public class UpdateGenreDtoValidator : AbstractValidator<CreateGenreDto>
    {
        public UpdateGenreDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Genre name is required.")
                .MaximumLength(50).WithMessage("Genre name cannot exceed 50 characters.");
        }
    }
}

