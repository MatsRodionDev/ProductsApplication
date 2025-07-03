using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProductsService.Domain.Models;

namespace ProductsService.Persistence
{
    public sealed class ApplicationDBContext(
        DbContextOptions<ApplicationDBContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Outbox> Outboxes { get; set; }
    }
}
