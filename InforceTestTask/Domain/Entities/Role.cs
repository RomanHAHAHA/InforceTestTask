using InforceTestTask.Domain.Abstractions;

namespace InforceTestTask.Domain.Entities;

public sealed class Role(int id, string name) : Enumeration<Role>(id, name)
{
    public static readonly Role User = new(1, nameof(User));

    public static readonly Role Admin = new(2, nameof(Admin));
    
    public ICollection<User> Users { get; set; } = [];
}