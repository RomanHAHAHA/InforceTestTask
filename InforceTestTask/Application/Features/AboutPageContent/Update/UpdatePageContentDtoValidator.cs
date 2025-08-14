using FluentValidation;

namespace InforceTestTask.Application.Features.AboutPageContent.Update;

public class UpdatePageContentDtoValidator : AbstractValidator<UpdatePageContentDto>
{
    public UpdatePageContentDtoValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description must be less than 1000 characters");

        RuleFor(x => x.Algorithm)
            .NotEmpty().WithMessage("Algorithm description is required")
            .MaximumLength(2000).WithMessage("Algorithm description is too long");

        RuleFor(x => x.Features)
            .NotEmpty().WithMessage("At least one feature is required")
            .Must(f => f.Count <= 10).WithMessage("Maximum 10 features allowed");

        RuleForEach(x => x.Features)
            .NotEmpty().WithMessage("Feature cannot be empty")
            .MaximumLength(100).WithMessage("Feature description is too long");
    }
}