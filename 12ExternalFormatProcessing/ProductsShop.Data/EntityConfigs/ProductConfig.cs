using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsShop.Models;

namespace ProductsShop.Data.EntityConfigs
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired();

            builder.Property(p => p.BuyerId)
                .IsRequired(false);

            builder.HasOne(p => p.Buyer)
                .WithMany(b => b.ProductsBought)
                .HasForeignKey(p => p.BuyerId);

            builder.HasOne(p => p.Seller)
                .WithMany(b => b.ProductsSold)
                .HasForeignKey(p => p.SellerId);
        }
    }
}