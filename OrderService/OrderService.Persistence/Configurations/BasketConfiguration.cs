using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Models;

namespace OrderService.Persistence.Configurations
{
    public class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.HasMany(b => b.BasketItems)
                .WithOne()
                .HasForeignKey(b => b.BasketId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(b => b.UserId)
                .IsUnique();
        }
    }
}
