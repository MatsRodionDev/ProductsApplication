namespace OrderService.Application.Common.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
