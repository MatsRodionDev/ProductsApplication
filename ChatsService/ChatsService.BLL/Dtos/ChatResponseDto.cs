using ChatsService.BLL.Models;

namespace ChatsService.BLL.Dtos
{
    public class ChatResponseDto
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public Guid BuyerId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int UnreadMessagesQuantity { get; set; }
        public Message? LastMessage { get; set; }
    }
}
