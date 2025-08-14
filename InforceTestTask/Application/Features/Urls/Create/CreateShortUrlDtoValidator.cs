using FluentValidation;

namespace InforceTestTask.Application.Features.Urls.Create;

public class CreateShortUrlDtoValidator : AbstractValidator<CreateShortUrlDto>
{
    public CreateShortUrlDtoValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("URL is required")
            .Must(BeValidUrl).WithMessage("Invalid URL format");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");
    }

    private bool BeValidUrl(string url) => Uri.TryCreate(url, UriKind.Absolute, out _);
}