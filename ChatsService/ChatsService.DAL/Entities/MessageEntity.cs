using ChatsService.DAL.Abstrcations;

namespace ChatsService.DAL.Entities
{
    public class MessageEntity : BaseEntity
    {
        public Guid SenderId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid ChatId { get; set; }
        public bool IsRead { get; set; }
        public ChatEntity? Chat { get; set; }
    }
}
