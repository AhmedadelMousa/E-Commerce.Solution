using E_Commerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Data.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
            builder.Property(p => p.PictureUrl).IsRequired();
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.StockQuantity).IsRequired();
            builder.Property(p => p.Size).IsRequired();
            builder.Property(p => p.Colors).IsRequired();
            builder.HasMany(p => p.users)
                .WithMany(p => p.products).UsingEntity<MakeReview>(
                j =>
                {
                    j.HasOne(e => e.appUser)
                    .WithMany()
                    .HasForeignKey(e => e.AppUserId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();
                    //j.HasOne(e => e.product)
                    //.WithMany()
                    //.HasForeignKey(e => e.ProductId)
                    //.OnDelete(DeleteBehavior.NoAction).IsRequired();

                   j.Property(p => p.Comment).IsRequired();
                    j.Property(p => p.NumberOfPoint).IsRequired();
                    j.Property(p => p.CreatedAt).IsRequired();

                }
                );
            //builder.HasOne(e => e.category)
            //    .WithMany().HasForeignKey(e => e.CategoryId)
            //    .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
