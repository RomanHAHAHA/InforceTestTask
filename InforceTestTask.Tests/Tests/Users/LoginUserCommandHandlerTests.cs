using System.Net;
using InforceTestTask.Application.Features.Users.Login;
using InforceTestTask.Domain.Entities;
using InforceTestTask.Domain.Interfaces;
using InforceTestTask.Persistence.Contexts;
using InforceTestTask.Tests.Common;
using Microsoft.Extensions.DependencyInjection;

namespace InforceTestTask.Tests.Tests.Users;

public class LoginUserCommandHandlerIntegrationTests
{
    private readonly AppDbContext _dbContext;
    private readonly LoginUserCommandHandler _handler;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserCommandHandlerIntegrationTests()
    {
        var serviceProvider = TestServices.ConfigureTestServices();

        _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        _passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();
        
        _handler = new LoginUserCommandHandler(
            _dbContext,
            serviceProvider.GetRequiredService<IJwtProvider>(),
            _passwordHasher);
        
        _dbContext.Database.EnsureCreated();
    }
    
    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFoundResponse()
    {
        // Arrange
        var command = new LoginUserCommand(new LoginUserDto
        {
            Email = "nonexistent@test.com",
            Password = "anypassword"
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.Status);
        Assert.Equal("User with such email not found", result.Error);
    }

    [Fact]
    public async Task Handle_IncorrectPassword_ReturnsBadRequest()
    {
        // Arrange
        var testUser = new User
        {
            Email = "test@test.com",
            HashedPassword = _passwordHasher.HashPassword("correctpassword")
        };
        await _dbContext.Users.AddAsync(testUser);
        await _dbContext.SaveChangesAsync();

        var command = new LoginUserCommand(new LoginUserDto
        {
            Email = "test@test.com",
            Password = "wrongpassword" 
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.Equal("Incorrect password", result.Error);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsValidToken()
    {
        // Arrange
        const string testPassword = "correctpassword";
        var testUser = new User
        {
            Email = "test@test.com",
            HashedPassword = _passwordHasher.HashPassword(testPassword)
        };
        await _dbContext.Users.AddAsync(testUser);
        await _dbContext.SaveChangesAsync();

        var command = new LoginUserCommand(new LoginUserDto
        {
            Email = "test@test.com",
            Password = testPassword
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);
    }

    [Fact]
    public async Task Handle_MultipleUsers_FindsCorrectUser()
    {
        // Arrange
        var users = new[]
        {
            new User { Email = "user1@test.com", HashedPassword = _passwordHasher.HashPassword("pass1") },
            new User { Email = "user2@test.com", HashedPassword = _passwordHasher.HashPassword("pass2") },
            new User { Email = "target@test.com", HashedPassword = _passwordHasher.HashPassword("targetpass") }
        };
        
        await _dbContext.Users.AddRangeAsync(users);
        await _dbContext.SaveChangesAsync();

        var command = new LoginUserCommand(new LoginUserDto
        {
            Email = "target@test.com",
            Password = "targetpass"
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.NotNull(result.Data);
    }
}