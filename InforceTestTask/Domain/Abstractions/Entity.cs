namespace InforceTestTask.Domain.Abstractions;

public abstract class Entity<TKey> 
{
    public TKey Id { get; set; }

    public DateTime CreatedAt { get; set; }
    
    protected Entity()
    {
        CreatedAt = DateTime.UtcNow;
    }
}