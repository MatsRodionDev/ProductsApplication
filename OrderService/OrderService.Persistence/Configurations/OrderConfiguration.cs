using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Models;

namespace OrderService.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.OrderItem)
                .WithOne()
                .HasForeignKey<OrderItem>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
