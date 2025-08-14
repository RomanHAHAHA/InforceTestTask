using InforceTestTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InforceTestTask.Persistence.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }

    public DbSet<ShortUrl> ShortUrls { get; set; }

    public DbSet<UrlContent> UrlContents { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}