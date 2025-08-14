using System.Text.Json.Serialization;

namespace InforceTestTask.Application.Features.AboutPageContent.Get;

public class AboutPageDto
{
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonPropertyName("algorithm")]
    public string Algorithm { get; set; } = string.Empty;

    [JsonPropertyName("features")]
    public List<string> Features { get; set; } = [];
}