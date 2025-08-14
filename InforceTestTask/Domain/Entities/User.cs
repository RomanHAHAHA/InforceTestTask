using InforceTestTask.Domain.Abstractions;

namespace InforceTestTask.Domain.Entities;

public class User : Entity<Guid>
{
    public string NickName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string HashedPassword { get; set; } = string.Empty;
    
    public int RoleId { get; set; }

    public Role? Role { get; set; }
    
    public ICollection<ShortUrl> ShortUrls { get; set; } = [];
}