using InforceTestTask.Domain.Abstractions;

namespace InforceTestTask.Domain.Entities;

public class ShortUrl : Entity<Guid>
{
    public string OriginalUrl { get; set; } = string.Empty;
    
    public string ShortCode { get; set; } = string.Empty;
    
    public Guid CreatorId { get; set; }
    
    public User? Creator { get; set; }
    
    public UrlContent? Content { get; set; }
}