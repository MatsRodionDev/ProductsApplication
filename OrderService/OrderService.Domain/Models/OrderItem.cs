using OrderService.Domain.Abstractions;

namespace OrderService.Domain.Models
{
    public class OrderItem : BaseModel
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProdcutName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
    }
}
