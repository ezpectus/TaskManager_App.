using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
// updated 27.01.26


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




// old version kept for reference
/*
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.TagId);

            builder.Property(t => t.TagName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.TagColor)
                .IsRequired()
                .HasMaxLength(7); // HEX

            builder.HasMany(t => t.TaskTags)
                .WithOne(tt => tt.Tag)
                .HasForeignKey(tt => tt.TagId);
        }
    }
 */

