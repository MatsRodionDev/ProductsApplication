using ChatsService.DAL.Abstrcations;
using ChatsService.DAL.Interfaces;

namespace ChatsService.DAL.Entities
{
    public class MessageEntity : BaseEntity, IAuditable
    {
        public Guid SenderId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid ChatId { get; set; }
        public bool IsReaded { get; set; }
        public ChatEntity? Chat { get; set; }
    }
}
