namespace InforceTestTask.Application.Features.Urls.GetInfoById;

public class UrlInfoDto
{
    public required Guid Id { get; init; }
    
    public required string OriginalUrl { get; init; }

    public required string ShortUrl { get; init; }
    
    public required string Content { get; init; }
        
    public required CreatorInfoDto Creator { get; init; }
}