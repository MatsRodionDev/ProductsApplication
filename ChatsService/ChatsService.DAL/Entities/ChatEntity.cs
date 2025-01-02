using ChatsService.DAL.Abstrcations;

namespace ChatsService.DAL.Entities
{
    public class ChatEntity : BaseEntity
    {
        public Guid SellerId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public Guid BuyerId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<MessageEntity> Messages { get; set; } = [];
    }
}
