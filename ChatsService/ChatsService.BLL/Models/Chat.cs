namespace ChatsService.BLL.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public Guid SellerId { get; set; }
        public Guid BuyerId { get; set; }
        public Guid ProductId { get; set; }
        public List<Message> Messages { get; set; } = [];
    }
}
