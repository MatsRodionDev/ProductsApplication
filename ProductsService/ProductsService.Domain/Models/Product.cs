using ProductsService.Domain.Abstrctions;

namespace ProductsService.Domain.Models
{
    public class Product : IBaseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int Price { get; set; }
        public Guid UserId { get; set; }

        public List<Image> Images { get; set; } = [];
        public List<Category> Categories { get; set; } = [];

        public Product Update(string name,
            string description,
            int quantity,
            int price)
        {
            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;

            return this;
        }
    }
}
