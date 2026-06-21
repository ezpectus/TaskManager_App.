using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastucture.Persistence.EntityConfigurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("tag_id");

        builder.Property(t => t.TagName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.TagColor)
            .IsRequired()
            .HasMaxLength(7);

        builder.HasMany(t => t.TaskTags)
            .WithOne(tt => tt.Tag)
            .HasForeignKey(tt => tt.TagId);
    }
}

