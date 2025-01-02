using ChatsService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatsService.DAL.Configurations
{
    public class ChatEntityConfiguration : IEntityTypeConfiguration<ChatEntity>
    {
        public void Configure(EntityTypeBuilder<ChatEntity> builder)
        {
            builder
                .HasMany(c => c.Messages)
                .WithOne(c => c.Chat)
                .HasForeignKey(c => c.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
