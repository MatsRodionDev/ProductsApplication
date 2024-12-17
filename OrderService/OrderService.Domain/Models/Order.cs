using OrderService.Domain.Abstractions;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Models
{
    public class Order : BaseModel
    {
        public Guid BuyerId { get; set; } 
        public Guid SellerId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal ToTalPrice { get; set; }
        public OrderItem? OrderItem { get; set; }
    }
}
