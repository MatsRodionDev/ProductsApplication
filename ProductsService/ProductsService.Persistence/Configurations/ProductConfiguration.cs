using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsService.Domain.Enums;
using ProductsService.Domain.Models;

namespace ProductsService.Persistence.Configurations
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .HasMany(p => p.Images)
                .WithOne()
                .HasForeignKey(i => i.UserId);

            builder
                .HasMany(p => p.Categories)
                .WithMany();
        }
    }

    internal sealed class OutboxConfiguration : IEntityTypeConfiguration<Outbox>
    {
        public void Configure(EntityTypeBuilder<Outbox> builder)
        {
            builder
                .Property(o => o.Content)
                .HasColumnType("jsonb");
        }
    }

    internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                Enum.GetValues<Categories>()
                    .Select(category => new Category
                    {
                        Name = category.ToString()
                    }));
        }
    }
}
