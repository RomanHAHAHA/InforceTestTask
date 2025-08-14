using FluentValidation.TestHelper;
using InforceTestTask.Application.Features.Urls.Create;

namespace InforceTestTask.Tests.Tests.Urls;

public class CreateShortUrlDtoValidatorTests
{
    private readonly CreateShortUrlDtoValidator _validator = new();

    [Theory]
    [InlineData("", "URL is required")]
    [InlineData("invalid-url", "Invalid URL format")]
    public void Validate_Url_ShouldFail_WhenInvalid(string url, string expectedError)
    {
        // Arrange
        var dto = new CreateShortUrlDto
        {
            Url = url,
            Description = "Valid description"
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Url)
              .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("https://example.com/path?query=param")]
    [InlineData("https://sub.domain.example.com")]
    public void Validate_Url_ShouldPass_WhenValid(string url)
    {
        // Arrange
        var dto = new CreateShortUrlDto
        {
            Url = url,
            Description = "Valid description"
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Url);
    }

    [Theory]
    [InlineData(null, "Description is required")]
    [InlineData("", "Description is required")]
    public void Validate_Description_ShouldFail_WhenInvalid(string description, string expectedError)
    {
        // Arrange
        var dto = new CreateShortUrlDto
        {
            Url = "https://valid.url",
            Description = description
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
              .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("Short description")]
    public void Validate_Description_ShouldPass_WhenValid(string description)
    {
        // Arrange
        var dto = new CreateShortUrlDto
        {
            Url = "https://valid.url",
            Description = description
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_AllFieldsEmpty_ShouldFailForAll()
    {
        // Arrange
        var dto = new CreateShortUrlDto
        {
            Url = "",
            Description = ""
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Url);
        result.ShouldHaveValidationErrorFor(x => x.Description);
        Assert.Equal(3, result.Errors.Count);
    }

    [Fact]
    public void Validate_ValidDto_ShouldPassAllValidations()
    {
        // Arrange
        var dto = new CreateShortUrlDto
        {
            Url = "https://example.com",
            Description = "Valid description"
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}