using InforceTestTask.Application.Features.Urls.GetAll;
using InforceTestTask.Domain.Entities;
using InforceTestTask.Persistence.Contexts;
using InforceTestTask.Tests.Common;
using Microsoft.Extensions.DependencyInjection;

namespace InforceTestTask.Tests.Tests.Urls;

public class GetAllShortUrlsQueryHandlerITests
{
    private readonly AppDbContext _dbContext;
    private readonly GetAllShortUrlsQueryHandler _handler;
    private readonly Guid _testUserId1 = Guid.NewGuid();
    private readonly Guid _testUserId2 = Guid.NewGuid();

    public GetAllShortUrlsQueryHandlerITests()
    {
        var serviceProvider = TestServices.ConfigureTestServices();

        _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        _handler = new GetAllShortUrlsQueryHandler(_dbContext);
        
        _dbContext.Database.EnsureCreated();
        
        AddTestUsers();
        AddTestUrls();
    }

    private void AddTestUsers()
    {
        var users = new[]
        {
            new User { Id = _testUserId1, Email = "user1@example.com", NickName = "user1" },
            new User { Id = _testUserId2, Email = "user2@example.com", NickName = "user2" }
        };

        _dbContext.Users.AddRange(users);
        _dbContext.SaveChanges();
    }

    private void AddTestUrls()
    {
        var now = DateTime.UtcNow;
        var urls = new[]
        {
            new ShortUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = "https://example.com/page1",
                ShortCode = "abc123",
                CreatorId = _testUserId1,
                CreatedAt = now.AddDays(-2)
            },
            new ShortUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = "https://example.com/page2",
                ShortCode = "def456",
                CreatorId = _testUserId2,
                CreatedAt = now.AddDays(-1)
            },
            new ShortUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = "https://example.com/page3",
                ShortCode = "ghi789",
                CreatorId = _testUserId1,
                CreatedAt = now
            }
        };

        _dbContext.ShortUrls.AddRange(urls);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Handle_ReturnsAllUrls()
    {
        // Arrange
        var command = new GetAllShortUrlsQuery();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.All(result, dto => Assert.NotNull(dto.OriginalUrl));
        Assert.All(result, dto => Assert.NotNull(dto.ShortUrl));
        Assert.All(result, dto => Assert.NotNull(dto.CreatedAt));
    }

    [Fact]
    public async Task Handle_ReturnsCorrectUrlMappings()
    {
        // Arrange
        var command = new GetAllShortUrlsQuery();
        var testUrl = _dbContext.ShortUrls.First();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        var matchedDto = result.First(dto => dto.Id == testUrl.Id);

        // Assert
        Assert.Equal(testUrl.OriginalUrl, matchedDto.OriginalUrl);
        Assert.Equal($"https://localhost:3000/{testUrl.ShortCode}", matchedDto.ShortUrl);
        Assert.Equal(testUrl.CreatedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm"), matchedDto.CreatedAt);
        Assert.Equal(testUrl.CreatorId, matchedDto.CreatorId);
    }

    [Fact]
    public async Task Handle_EmptyDatabase_ReturnsEmptyList()
    {
        // Arrange
        _dbContext.ShortUrls.RemoveRange(_dbContext.ShortUrls);
        await _dbContext.SaveChangesAsync();
        
        var command = new GetAllShortUrlsQuery();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}