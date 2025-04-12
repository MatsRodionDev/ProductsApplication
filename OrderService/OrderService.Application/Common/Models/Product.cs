namespace OrderService.Application.Common.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid UserId { get; set; }

        public List<Image> Images { get; set; } = [];
    }
}
