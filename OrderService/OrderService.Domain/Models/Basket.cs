using OrderService.Domain.Abstractions;

namespace OrderService.Domain.Models
{
    public class Basket : BaseModel
    {
        public Guid UserId { get; set; }
        public List<BasketItem> BasketItems { get; set; } = [];

        public Basket(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
        }

    }
}
