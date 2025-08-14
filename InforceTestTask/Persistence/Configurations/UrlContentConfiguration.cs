using InforceTestTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InforceTestTask.Persistence.Configurations;

public class UrlContentConfiguration : IEntityTypeConfiguration<UrlContent>
{
    public void Configure(EntityTypeBuilder<UrlContent> builder)
    {
        builder.ToTable("UrlContents");

        builder.HasKey(uc => uc.UrlId);
        
        builder.Property(uc => uc.Description).IsRequired();
        
        builder.HasOne(uc => uc.ShortUrl)
            .WithOne(su => su.Content)
            .HasForeignKey<UrlContent>(s => s.UrlId);
    }
}