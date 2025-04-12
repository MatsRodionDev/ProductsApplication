using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.DAL.Outbox;

namespace UserService.DAL.Configurations
{
    public class OutboxMessageEntityConfiguration : IEntityTypeConfiguration<OutboxMessageEntity>
    {
        public void Configure(EntityTypeBuilder<OutboxMessageEntity> builder)
        {
            builder
                .Property(m => m.Content)
                .HasColumnType("jsonb");

            builder
                .HasIndex(m => m.ProcessedOnUtc);

            builder
                .HasIndex(m => m.OccuredOnUtc);
        }
    }
}
