namespace InforceTestTask.Application.Features.Urls.GetInfoById;

public class CreatorInfoDto
{
    public required Guid Id { get; init; }

    public required string NickName { get; init; }
    
    public required string Email { get; init; }
    
    public required string Role { get; init; }
    
    public required string RegisteredDate { get; init; }
}