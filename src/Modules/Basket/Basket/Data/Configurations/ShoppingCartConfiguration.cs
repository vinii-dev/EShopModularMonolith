using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.Data.Configurations;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.UserName)
            .IsUnique();

        builder.Property(c => c.UserName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(c => c.Items)
            .WithOne()
            .HasForeignKey(i => i.ShoppingCartId);
    }
}
