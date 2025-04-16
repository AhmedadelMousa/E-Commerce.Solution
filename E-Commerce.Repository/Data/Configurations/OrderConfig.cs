using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Order_Aggregrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Data.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status)
                .HasConversion(
                Ostatus => Ostatus.ToString(),
                Ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), Ostatus)
                );

            builder.Property(o => o.paymentMethod)
                .HasConversion(
                Pstatus => Pstatus.ToString(),
                Pstatus => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), Pstatus)
                );
            builder.HasOne(o=>o.User).WithMany().HasForeignKey(o => o.AppUserId).OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(o => o.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());
            builder.Property(o => o.SubTotal)
                .HasColumnType("decimal(18,2)");
            builder.HasOne(o => o.DeliveryMethod)
               .WithMany()
                 .HasForeignKey(o => o.DeliveryMethodId)
               .OnDelete(DeleteBehavior.SetNull);
















        }
    }
}
