using InforceTestTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InforceTestTask.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.NickName).HasMaxLength(255);
        builder.Property(u => u.Email).HasMaxLength(255);
        builder.Property(u => u.HashedPassword);
        builder.Property(u => u.CreatedAt);
        
        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.ShortUrls)
            .WithOne(su => su.Creator)
            .HasForeignKey(x => x.CreatorId);
    }
}