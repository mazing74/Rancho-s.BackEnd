using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rancho_s.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.Repository.Data.Configurations
{
    public class OptionGroupConfiguration : IEntityTypeConfiguration<OptionGroup>
    {
        public void Configure(EntityTypeBuilder<OptionGroup> builder)
        {
            builder.ToTable("OptionGroups");

            builder.Property(og => og.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(og => og.NameAr)
                .HasMaxLength(100);

            builder.Property(og => og.SelectionType)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("single");

            // One Product → Many OptionGroups
            // Delete the product → delete its option groups too
            builder.HasOne(og => og.Product)
                .WithMany(p => p.OptionGroups)
                .HasForeignKey(og => og.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
