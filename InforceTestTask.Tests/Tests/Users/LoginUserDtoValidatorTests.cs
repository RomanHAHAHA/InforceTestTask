using FluentValidation.TestHelper;
using InforceTestTask.Application.Features.Users.Login;

namespace InforceTestTask.Tests.Tests.Users;

public class LoginUserDtoValidatorTests
{
    private readonly LoginUserDtoValidator _validator = new();

    public static IEnumerable<object[]> InvalidPasswordData()
    {
        yield return ["", "Password is required"];
        yield return ["short", "Password must be at least 10 symbols long"];
        yield return [new string('x', 2000), "Password cannot exceed 100 symbols"];
    }
    
    public static IEnumerable<object[]> InvalidEmailData()
    {
        yield return ["", "Email is required"];
        yield return ["a@b", "Email must be at least 5 symbols long"];
        yield return ["invalid-email", "Invalid email format"];
        yield return [new string('x', 51), "Email cannot exceed 50 symbols"];
    }
    
    [Theory]
    [MemberData(nameof(InvalidEmailData))]
    public void Validate_Email_ShouldFail_WhenInvalid(string email, string expectedErrorMessage)
    {
        // Arrange
        var dto = new LoginUserDto
        {
            Email = email,
            Password = "ValidPassword1!"
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage(expectedErrorMessage);
    }

    [Theory]
    [InlineData("valid@example.com")]
    [InlineData("user.name+tag@domain.co.uk")]
    [InlineData("email@sub.domain.com")]
    public void Validate_Email_ShouldPass_WhenValid(string email)
    {
        // Arrange
        var dto = new LoginUserDto
        {
            Email = email,
            Password = "ValidPassword1!"
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [MemberData(nameof(InvalidPasswordData))]
    public void Validate_Password_ShouldFail_WhenInvalid(string password, string expectedErrorMessage)
    {
        // Arrange
        var dto = new LoginUserDto
        {
            Email = "valid@example.com",
            Password = password
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage(expectedErrorMessage);
    }

    [Theory]
    [InlineData("ValidPass123!")]
    [InlineData("AnotherGood1!")]
    [InlineData("TenSymbols1!")]
    public void Validate_Password_ShouldPass_WhenValid(string password)
    {
        // Arrange
        var dto = new LoginUserDto
        {
            Email = "valid@example.com",
            Password = password
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_AllFields_ShouldFail_WhenAllEmpty()
    {
        // Arrange
        var dto = new LoginUserDto
        {
            Email = "",
            Password = ""
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
        Assert.Equal(5, result.Errors.Count);
    }

    [Fact]
    public void Validate_ValidDto_ShouldPassAllValidations()
    {
        // Arrange
        var dto = new LoginUserDto
        {
            Email = "valid@example.com",
            Password = "ValidPassword1!"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}