using FluentValidation;
using GameStoreApi.DTOs;

namespace GameStoreApi.Validators;

public class CreatePublisherDtoValidator : AbstractValidator<CreatePublisherDto>
{
    public CreatePublisherDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Publisher name is required.")
            .MaximumLength(100).WithMessage("Publisher name cannot exceed 100 characters.");
    }
    
    public class UpdatePublisherDtoValidator : AbstractValidator<CreatePublisherDto>
    {
        public UpdatePublisherDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Publisher name is required.")
                .MaximumLength(100).WithMessage("Publisher name cannot exceed 100 characters.");
        }
    }
}