namespace InforceTestTask.Application.Features.AboutPageContent.Update;

public class UpdatePageContentDto
{
    public string Description { get; init; } = string.Empty;
    
    public string Algorithm { get; init; } =  string.Empty;
    
    public List<string> Features { get; init; } = [];
}