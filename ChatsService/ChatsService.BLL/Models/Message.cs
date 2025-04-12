namespace ChatsService.BLL.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid ChatId { get; set; }
        public bool IsRead { get; set; }
    }
}
