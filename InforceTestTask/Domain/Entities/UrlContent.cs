namespace InforceTestTask.Domain.Entities;

public class UrlContent
{
    public Guid UrlId { get; set; }
    
    public ShortUrl? ShortUrl { get; set; }

    public string Description { get; set; } = string.Empty;
}