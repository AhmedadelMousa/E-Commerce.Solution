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
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(OrderItem => OrderItem.Product,
               product => product.WithOwner());

            builder.Property(o => o.Price)
                .HasColumnType("decimal(18)");
        }
    }
}
