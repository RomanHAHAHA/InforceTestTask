using System.Net;
using InforceTestTask.Application.Features.Urls.GetInfoById;
using InforceTestTask.Domain.Entities;
using InforceTestTask.Persistence.Contexts;
using InforceTestTask.Tests.Common;
using Microsoft.Extensions.DependencyInjection;

namespace InforceTestTask.Tests.Tests.Urls;

public class GetShortUrlInfoQueryHandlerITests
{
    private readonly AppDbContext _dbContext;
    private readonly GetShortUrlInfoQueryHandler _handler;
    private readonly Guid _testUserId = Guid.NewGuid();
    private readonly Guid _testUrlId = Guid.NewGuid();

    public GetShortUrlInfoQueryHandlerITests()
    {
        var serviceProvider = TestServices.ConfigureTestServices();

        _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        _handler = new GetShortUrlInfoQueryHandler(_dbContext);
        
        _dbContext.Database.EnsureCreated();
        
        AddTestUser();
        AddTestUrl();
    }

    private void AddTestUser()
    {
        var testUser = new User
        {
            Id = _testUserId,
            Email = "testuser@example.com",
            NickName = "Test User",
            HashedPassword = "hashedpassword",
            RoleId = Role.User.Id,
            CreatedAt = new DateTime(2025, 1, 1),
        };

        _dbContext.Users.Add(testUser);
        _dbContext.SaveChanges();
    }

    private void AddTestUrl()
    {
        var shortUrl = new ShortUrl
        {
            Id = _testUrlId,
            OriginalUrl = "https://original-url.com",
            ShortCode = "testcode",
            CreatorId = _testUserId,
            Content = new UrlContent { Description = "Test description" },
            CreatedAt = new DateTime(2025, 1, 1),
        };

        _dbContext.ShortUrls.Add(shortUrl);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Handle_ExistingUrl_ReturnsCorrectUrlInfo()
    {
        // Arrange
        var command = new GetShortUrlInfoQuery(_testUrlId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.Status);
        var urlInfo = result.Data;
        
        Assert.NotNull(urlInfo);
        Assert.Equal(_testUrlId, urlInfo.Id);
        Assert.Equal("https://original-url.com", urlInfo.OriginalUrl);
        Assert.Equal("Test description", urlInfo.Content);
        
        Assert.NotNull(urlInfo.Creator);
        Assert.Equal(_testUserId, urlInfo.Creator.Id);
        Assert.Equal("Test User", urlInfo.Creator.NickName);
        Assert.Equal("testuser@example.com", urlInfo.Creator.Email);
        Assert.Equal("User", urlInfo.Creator.Role);
        Assert.Equal("01.01.2025", urlInfo.Creator.RegisteredDate);
    }

    [Fact]
    public async Task Handle_NonExistingUrl_ReturnsNotFound()
    {
        // Arrange
        var nonExistingUrlId = Guid.NewGuid();
        var command = new GetShortUrlInfoQuery(nonExistingUrlId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.Status);
        Assert.Equal("Url was not found", result.Error);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task Handle_UrlWithoutCreator_ReturnsError()
    {
        // Arrange
        var urlWithoutCreator = new ShortUrl
        {
            Id = Guid.NewGuid(),
            OriginalUrl = "https://no-creator.com",
            ShortCode = "nocreator",
            Content = new UrlContent { Description = "No creator description" }
        };

        _dbContext.ShortUrls.Add(urlWithoutCreator);
        await _dbContext.SaveChangesAsync();

        var command = new GetShortUrlInfoQuery(urlWithoutCreator.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_UrlWithoutContent_ReturnsCorrectInfo()
    {
        // Arrange
        var urlWithoutContent = new ShortUrl
        {
            Id = Guid.NewGuid(),
            OriginalUrl = "https://no-content.com",
            ShortCode = "nocontent",
            CreatorId = _testUserId
        };

        _dbContext.ShortUrls.Add(urlWithoutContent);
        await _dbContext.SaveChangesAsync();

        var command = new GetShortUrlInfoQuery(urlWithoutContent.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.NotNull(result.Data);
        
        var urlInfo = result.Data;
        Assert.Equal("https://no-content.com", urlInfo.OriginalUrl);
        Assert.Null(urlInfo.Content);
        Assert.NotNull(urlInfo.Creator);
    }
}