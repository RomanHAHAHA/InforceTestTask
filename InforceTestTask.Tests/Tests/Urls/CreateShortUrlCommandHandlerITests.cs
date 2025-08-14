using System.Net;
using InforceTestTask.Application.Features.Urls.Create;
using InforceTestTask.Domain.Entities;
using InforceTestTask.Domain.Interfaces;
using InforceTestTask.Persistence.Contexts;
using InforceTestTask.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InforceTestTask.Tests.Tests.Urls;

public class CreateShortUrlCommandHandlerITests
{
    private readonly AppDbContext _dbContext;
    private readonly CreateShortUrlCommandHandler _handler;
    private readonly IUrlShortener _urlShortener;
    private readonly Guid _testUserId = Guid.NewGuid();

    public CreateShortUrlCommandHandlerITests()
    {
        var serviceProvider = TestServices.ConfigureTestServices();

        _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        _urlShortener = serviceProvider.GetRequiredService<IUrlShortener>();
        
        _handler = new CreateShortUrlCommandHandler(
            _dbContext,
            _urlShortener);
        
        _dbContext.Database.EnsureCreated();
        
        AddTestUser(_testUserId);
    }

    private void AddTestUser(Guid userId)
    {
        var testUser = new User
        {
            Id = userId,
            Email = "testuser@example.com",
            NickName = "testuser",
            HashedPassword = "hashedpassword" 
        };

        _dbContext.Users.Add(testUser);
        _dbContext.SaveChanges();
    }
    
    [Fact]
    public async Task Handle_UrlAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var existingUrl = "https://existing-url.com";
        var testShortUrl = new ShortUrl
        {
            OriginalUrl = existingUrl,
            ShortCode = _urlShortener.GenerateShortCode(existingUrl),
            CreatorId = _testUserId,
            Content = new UrlContent { Description = "Test description" }
        };
        
        await _dbContext.ShortUrls.AddAsync(testShortUrl);
        await _dbContext.SaveChangesAsync();

        var command = new CreateShortUrlCommand(
            new CreateShortUrlDto
            {
                Url = existingUrl, 
                Description = "New description"
            },
            _testUserId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.Equal("URL already exists", result.Description);
    }

    [Fact]
    public async Task Handle_ValidUrl_CreatesShortUrl()
    {
        // Arrange
        const string newUrl = "https://new-url.com";
        var command = new CreateShortUrlCommand(
            new CreateShortUrlDto { Url = newUrl, Description = "New URL description" },
            _testUserId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.Status);
        
        var createdUrl = await _dbContext.ShortUrls
            .FirstOrDefaultAsync(u => u.OriginalUrl == newUrl);
        
        Assert.NotNull(createdUrl);
        Assert.Equal(newUrl, createdUrl.OriginalUrl);
        Assert.Equal(_testUserId, createdUrl.CreatorId);
        Assert.Equal("New URL description", createdUrl.Content!.Description);
        Assert.NotEmpty(createdUrl.ShortCode);
    }

    [Fact]
    public async Task Handle_ValidUrl_GeneratesUniqueShortCode()
    {
        // Arrange
        const string url1 = "https://url1.com";
        const string url2 = "https://url2.com";
        
        var command1 = new CreateShortUrlCommand(
            new CreateShortUrlDto { Url = url1, Description = "URL 1" },
            _testUserId);
        
        var command2 = new CreateShortUrlCommand(
            new CreateShortUrlDto { Url = url2, Description = "URL 2" },
            _testUserId);

        // Act
        await _handler.Handle(command1, CancellationToken.None);
        await _handler.Handle(command2, CancellationToken.None);

        // Assert
        var shortUrls = await _dbContext.ShortUrls.ToListAsync();
        Assert.Equal(2, shortUrls.Count);
        Assert.NotEqual(shortUrls[0].ShortCode, shortUrls[1].ShortCode);
    }

    [Fact]
    public async Task Handle_ValidUrl_IncludesDescription()
    {
        // Arrange
        const string testUrl = "https://test-with-description.com";
        const string testDescription = "This is a detailed description";
        
        var command = new CreateShortUrlCommand(
            new CreateShortUrlDto { Url = testUrl, Description = testDescription },
            _testUserId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var createdUrl = await _dbContext.ShortUrls
            .Include(u => u.Content)
            .FirstOrDefaultAsync(u => u.OriginalUrl == testUrl);
        
        Assert.NotNull(createdUrl);
        Assert.Equal(testDescription, createdUrl.Content!.Description);
    }
}