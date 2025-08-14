using InforceTestTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InforceTestTask.Persistence.Configurations;

public class ShortUrlConfiguration : IEntityTypeConfiguration<ShortUrl>
{
    public void Configure(EntityTypeBuilder<ShortUrl> builder)
    {
        builder.ToTable("ShortUrls");
        
        builder.HasKey(su => su.Id);
        
        builder.Property(su => su.OriginalUrl).IsRequired();
        builder.Property(su => su.ShortCode).IsRequired();
        builder.Property(su => su.CreatorId).IsRequired();
        
        builder.HasIndex(su => su.OriginalUrl).IsUnique();
        builder.HasIndex(su => su.ShortCode).IsUnique();
        
        builder.HasOne(su => su.Creator)
            .WithMany(u => u.ShortUrls)
            .HasForeignKey(x => x.CreatorId);

        builder.HasOne(su => su.Content)
            .WithOne(c => c.ShortUrl)
            .HasForeignKey<UrlContent>(s => s.UrlId);
    }
}