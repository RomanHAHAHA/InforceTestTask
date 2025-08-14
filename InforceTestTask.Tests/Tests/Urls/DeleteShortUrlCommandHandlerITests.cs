using System.Net;
using InforceTestTask.Application.Features.Urls.Delete;
using InforceTestTask.Domain.Entities;
using InforceTestTask.Persistence.Contexts;
using InforceTestTask.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InforceTestTask.Tests.Tests.Urls;

public class DeleteShortUrlCommandHandlerITests
{
    private readonly AppDbContext _dbContext;
    private readonly DeleteShortUrlCommandHandler _handler;
    private readonly Guid _adminUserId = Guid.NewGuid();
    private readonly Guid _regularUserId = Guid.NewGuid();
    private readonly Guid _otherUserId = Guid.NewGuid();

    public DeleteShortUrlCommandHandlerITests()
    {
        var serviceProvider = TestServices.ConfigureTestServices();

        _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        _handler = new DeleteShortUrlCommandHandler(_dbContext);
        
        _dbContext.Database.EnsureCreated();
        
        AddTestUser(_adminUserId, Role.Admin.Id);
        AddTestUser(_regularUserId, Role.User.Id);
        AddTestUser(_otherUserId, Role.User.Id);
    }
    
    private void AddTestUser(Guid userId, int roleId)
    {
        var testUser = new User
        {
            Id = userId,
            Email = $"{userId}@example.com",
            NickName = $"user-{userId}",
            HashedPassword = "hashedpassword",
            RoleId = roleId
        };

        _dbContext.Users.Add(testUser);
        _dbContext.SaveChanges();
    }

    private async Task<ShortUrl> AddTestUrl(Guid creatorId, string url = "https://test-url.com")
    {
        var shortUrl = new ShortUrl
        {
            OriginalUrl = url,
            ShortCode = "testcode",
            CreatorId = creatorId,
            Content = new UrlContent { Description = "Test description" }
        };

        await _dbContext.ShortUrls.AddAsync(shortUrl);
        await _dbContext.SaveChangesAsync();

        return shortUrl;
    }

    [Fact]
    public async Task Handle_InvalidUserId_ReturnsBadRequest()
    {
        // Arrange
        var invalidUserId = Guid.NewGuid();
        var command = new DeleteShortUrlCommand(Guid.NewGuid(), invalidUserId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.Equal("Incorrect user id", result.Description);
    }

    [Fact]
    public async Task Handle_InvalidUrlId_ReturnsBadRequest()
    {
        // Arrange
        var invalidUrlId = Guid.NewGuid();
        var command = new DeleteShortUrlCommand(invalidUrlId, _regularUserId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.Equal("Incorrect URL id", result.Description);
    }

    [Fact]
    public async Task Handle_RegularUserTriesDeleteOthersUrl_ReturnsForbidden()
    {
        // Arrange
        var url = await AddTestUrl(_otherUserId);
        var command = new DeleteShortUrlCommand(url.Id, _regularUserId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, result.Status);
        Assert.Equal("You have no permission to delete other users URLs", result.Description);
        
        var urlExists = await _dbContext.ShortUrls.AnyAsync(u => u.Id == url.Id);
        Assert.True(urlExists);
    }

    [Fact]
    public async Task Handle_RegularUserDeletesOwnUrl_Succeeds()
    {
        // Arrange
        var url = await AddTestUrl(_regularUserId);
        var command = new DeleteShortUrlCommand(url.Id, _regularUserId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.Status);
        
        var urlExists = await _dbContext.ShortUrls.AnyAsync(u => u.Id == url.Id);
        Assert.False(urlExists);
    }

    [Fact]
    public async Task Handle_AdminDeletesAnyUrl_Succeeds()
    {
        // Arrange
        var url = await AddTestUrl(_otherUserId);
        var command = new DeleteShortUrlCommand(url.Id, _adminUserId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.Status);
        
        var urlExists = await _dbContext.ShortUrls.AnyAsync(u => u.Id == url.Id);
        Assert.False(urlExists);
    }

    [Fact]
    public async Task Handle_AdminDeletesOwnUrl_Succeeds()
    {
        // Arrange
        var url = await AddTestUrl(_adminUserId);
        var command = new DeleteShortUrlCommand(url.Id, _adminUserId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.Status);
        
        var urlExists = await _dbContext.ShortUrls.AnyAsync(u => u.Id == url.Id);
        Assert.False(urlExists);
    }
}