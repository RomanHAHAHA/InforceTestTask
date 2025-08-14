using System.Net;
using InforceTestTask.Application.Features.Users.Register;
using InforceTestTask.Domain.Entities;
using InforceTestTask.Persistence.Contexts;
using InforceTestTask.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InforceTestTask.Tests.Tests.Users;

[Collection("Sequential Tests")]
public class UserRegistrationTests
{
    private readonly RegisterUserCommandHandler _handler;
    private readonly AppDbContext _appDbContext;

    public UserRegistrationTests()
    {
        var serviceProvider = TestServices.ConfigureTestServices();
        _appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
        
        _appDbContext.Database.EnsureDeleted();
        _appDbContext.Database.EnsureCreated();
        
        _handler = serviceProvider.GetRequiredService<RegisterUserCommandHandler>();
    }
    
    [Fact]
    public async Task Handle_ValidCommand_ShouldRegisterUser()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            NickName = "NotPhoenix",
            Email = "simonovroman200513@gmail.com",
            Password = "1111111111",
            PasswordConfirm = "1111111111",
            IsAdmin = true
        };
        var command = new RegisterUserCommand(dto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var userInDb = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == command.Dto.Email);
        
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.NotNull(userInDb);
        Assert.Equal(command.Dto.Email, userInDb.Email);
        Assert.NotNull(userInDb.HashedPassword);
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ShouldThrowException()
    {
        // Arrange
        var existingUser = new User { Email = "existing@test.com" };
        await _appDbContext.Users.AddAsync(existingUser);
        await _appDbContext.SaveChangesAsync();

        var dto = new RegisterUserDto
        {
            NickName = "NotPhoenix",
            Email = "existing@test.com",
            Password = "1111111111",
            PasswordConfirm = "1111111111",
            IsAdmin = true
        };
        var command = new RegisterUserCommand(dto);

        // Act 
        var response = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        var usersCount = await _appDbContext.Users.CountAsync();
        
        Assert.Equal(1, usersCount);
        Assert.Equal(HttpStatusCode.Conflict, response.Status);
    }
}