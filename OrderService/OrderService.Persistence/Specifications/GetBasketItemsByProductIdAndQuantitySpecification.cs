using OrderService.Domain.Models;

namespace OrderService.Persistence.Specifications
{
    internal class GetBasketItemsByProductIdAndQuantitySpecification : Specification<BasketItem>
    {
        public GetBasketItemsByProductIdAndQuantitySpecification(
            Guid productId,
            int Quantity)
        {
            AddCriteria(i => i.ProductId == productId);
            AddCriteria(i => i.Quantity > Quantity);
        }
    }
}
