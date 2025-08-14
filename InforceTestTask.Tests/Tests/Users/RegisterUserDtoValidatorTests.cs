using FluentValidation.TestHelper;
using InforceTestTask.Application.Features.Users.Register;

namespace InforceTestTask.Tests.Tests.Users;

public class RegisterUserDtoValidatorTests
{
    private readonly RegisterUserDtoValidator _validator = new();

    public static IEnumerable<object[]> InvalidPasswordData()
    {
        yield return ["", "Password is required"];
        yield return ["short", "Password must be at least 10 symbols long"];
        yield return [new string('x', 2000), "Password cannot exceed 100 symbols"];
    }
    
    public static IEnumerable<object[]> InvalidNickNameData()
    {
        yield return ["", "Nickname is required"];
        yield return ["ab", "Nickname must be at least 3 symbols long"];
        yield return [new string('x', 51), "Nickname cannot exceed 50 symbols"];
    }
    
    public static IEnumerable<object[]> InvalidEmailData()
    {
        yield return ["", "Email is required"];
        yield return ["a@b", "Email must be at least 5 symbols long"];
        yield return ["invalid-email", "Invalid email format"];
        yield return [new string('x', 51), "Email cannot exceed 50 symbols"];
    }
    
    [Theory]
    [MemberData(nameof(InvalidNickNameData))]
    public void Validate_NickName_ShouldFail_WhenInvalid(string nickName, string expectedErrorMessage)
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            NickName = nickName,
            Email = "valid@example.com",
            Password = "ValidPassword1!",
            PasswordConfirm = "ValidPassword1!"
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NickName)
              .WithErrorMessage(expectedErrorMessage);
    }

    [Theory]
    [InlineData("ValidNick")]
    [InlineData("Nick123")]
    [InlineData("Good_Nick")]
    public void Validate_NickName_ShouldPass_WhenValid(string nickName)
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            NickName = nickName,
            Email = "valid@example.com",
            Password = "ValidPassword1!",
            PasswordConfirm = "ValidPassword1!"
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.NickName);
    }

    [Theory]
    [MemberData(nameof(InvalidEmailData))]
    public void Validate_Email_ShouldFail_WhenInvalid(string email, string expectedErrorMessage)
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            NickName = "ValidNick",
            Email = email,
            Password = "ValidPassword1!",
            PasswordConfirm = "ValidPassword1!"
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
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
        var dto = new RegisterUserDto
        {
            NickName = "ValidNick",
            Email = email,
            Password = "ValidPassword1!",
            PasswordConfirm = "ValidPassword1!"
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
        var dto = new RegisterUserDto
        {
            NickName = "ValidNick",
            Email = "valid@example.com",
            Password = password,
            PasswordConfirm = password
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
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
        var dto = new RegisterUserDto
        {
            NickName = "ValidNick",
            Email = "valid@example.com",
            Password = password,
            PasswordConfirm = password
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("", "Password confirmation is required")]
    [InlineData("Different1!", "Passwords must match")]
    public void Validate_PasswordConfirm_ShouldFail_WhenInvalid(string passwordConfirm, string expectedErrorMessage)
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            NickName = "ValidNick",
            Email = "valid@example.com",
            Password = "ValidPassword1!",
            PasswordConfirm = passwordConfirm
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PasswordConfirm)
              .WithErrorMessage(expectedErrorMessage);
    }

    [Fact]
    public void Validate_PasswordConfirm_ShouldPass_WhenMatchesPassword()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            NickName = "ValidNick",
            Email = "valid@example.com",
            Password = "ValidPassword1!",
            PasswordConfirm = "ValidPassword1!"
        };

        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PasswordConfirm);
    }

    [Fact]
    public void Validate_AllFields_ShouldFail_WhenAllEmpty()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            NickName = "",
            Email = "",
            Password = "",
            PasswordConfirm = ""
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NickName);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldHaveValidationErrorFor(x => x.PasswordConfirm);
        Assert.Equal(8, result.Errors.Count);
    }
}