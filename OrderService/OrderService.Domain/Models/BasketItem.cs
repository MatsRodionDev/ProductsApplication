using OrderService.Domain.Abstractions;

namespace OrderService.Domain.Models
{
    public class BasketItem : BaseModel
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public BasketItem(
            Guid basketId,
            Guid productId,
            int quantity)
        {
            Id = Guid.NewGuid();
            BasketId = basketId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
