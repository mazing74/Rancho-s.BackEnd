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
    public class OrderItemOptionConfiguration : IEntityTypeConfiguration<OrderItemOption>
    {
        public void Configure(EntityTypeBuilder<OrderItemOption> builder)
        {
            builder.ToTable("OrderItemOptions");

            builder.Property(o => o.OptionGroupName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.OptionName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.AdditionalPrice)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.OrderItem)
                .WithMany(oi => oi.Options)
                .HasForeignKey(o => o.OrderItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
