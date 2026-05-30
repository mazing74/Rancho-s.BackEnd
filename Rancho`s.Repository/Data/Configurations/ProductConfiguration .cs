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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
           builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
           .IsRequired()
           .HasMaxLength(200);

            builder.Property(p => p.NameAr)
            .HasMaxLength(200);


            builder.Property(p => p.Description)
            .HasMaxLength(2000);


            builder.Property(p => p.DescriptionAr)
                .HasMaxLength(2000);

            // Price - decimal(18,2) means 18 digits total, 2 after decimal point
            // This is the standard for money. Never use float for money - rounding errors!
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.ImageUrl)
              .HasMaxLength(500);

            builder.Property(p => p.IsActive)
                .HasDefaultValue(true);

            builder.Property(p => p.IsAvailable)
                .HasDefaultValue(true);

            builder.Property(p => p.IsFeatured)
                .HasDefaultValue(false);

            builder.Property(p => p.DisplayOrder)
                .HasDefaultValue(0);

            builder.HasIndex(p => p.CategoryId);

            builder.HasIndex(p => p.IsActive);

            builder.HasOne(p => p.Category).
                WithMany(c => c.Products).
                HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            ;

        }
    }
}
