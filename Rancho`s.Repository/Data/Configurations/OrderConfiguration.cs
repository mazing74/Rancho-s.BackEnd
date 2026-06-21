using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rancho_s.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.Repository.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(30);

            // Unique index — no two orders can have same number
            builder.HasIndex(o => o.OrderNumber)
                .IsUnique();

            // Store enums as strings in DB — much easier to read in SSMS
            // Instead of seeing "1" you see "Pending"
            builder.Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(o => o.OrderType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(o => o.PaymentMethod)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(o => o.PaymentStatus)
                .HasConversion<string>()
                .HasMaxLength(20);

            // All money columns — decimal(18,2)
            builder.Property(o => o.SubTotal)
                .HasColumnType("decimal(18,2)");
            builder.Property(o => o.DeliveryFee)
                .HasColumnType("decimal(18,2)");
            builder.Property(o => o.DiscountAmount)
                .HasColumnType("decimal(18,2)");
            builder.Property(o => o.TaxAmount)
                .HasColumnType("decimal(18,2)");
            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.DeliveryAddress)
                .HasMaxLength(500);
            builder.Property(o => o.DeliveryNotes)
                .HasMaxLength(500);
            builder.Property(o => o.SpecialInstructions)
                .HasMaxLength(1000);
            builder.Property(o => o.RejectionReason)
                .HasMaxLength(500);
            builder.Property(o => o.TableNumber)
                .HasMaxLength(20);
                builder.HasOne(o => o.Customer)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); 

            // Indexes for common queries
            builder.HasIndex(o => o.CustomerId);
            builder.HasIndex(o => o.Status);
            builder.HasIndex(o => o.BranchId);
            builder.HasIndex(o => o.CreatedAt);
        }
    }
}
