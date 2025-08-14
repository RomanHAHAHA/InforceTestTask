namespace InforceTestTask.Application.Features.Urls.GetAll;

public class ShortUrlTableDto
{
    public required Guid Id { get; init; }
    
    public required Guid CreatorId { get; init; }
    
    public required string OriginalUrl { get; init; }
    
    public required string ShortUrl { get; init; }
    
    public required string CreatedAt { get; init; }
}