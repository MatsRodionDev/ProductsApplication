using OrderService.Application.Common.Intefaces;
using OrderService.Application.Common.Models;
using OrderService.Domain.Exceptions;

namespace OrderService.Infrastructure.Grpc
{
    public class ProductService : IProductService
    {
        private readonly List<Product> products =
        [
            new Product { Id = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"), Name = "Product 1", Description = "Description of Product 1", Quantity = 10, Price = 9.99m, UserId = Guid.Parse("1d3f0e1c-5f6a-45c3-b8b2-5c3f7a1e0c5b") },
            new Product { Id = Guid.Parse("a7f5c1de-4d5e-4d2f-bb5f-7c2b5e3e1b9e"), Name = "Product 2", Description = "Description of Product 2", Quantity = 20, Price = 19.99m, UserId = Guid.Parse("2e5d4c2b-6f3a-4b5a-9b5b-e3a5b3c1d7f9") },
            new Product { Id = Guid.Parse("c9b1e5f4-64d8-4e5b-8c4c-5a1d9c6c7b1a"), Name = "Product 3", Description = "Description of Product 3", Quantity = 5, Price = 29.99m, UserId = Guid.Parse("3f6b1d5e-2a7f-4f36-bb2c-6c2e1d9e1c4f") },
            new Product { Id = Guid.Parse("b2f4e6c7-9a1c-4b5d-b8a7-4f3b2c1e5d7e"), Name = "Product 4", Description = "Description of Product 4", Quantity = 15, Price = 39.99m, UserId = Guid.Parse("4e1c2d3f-9b5f-4a5c-8b2e-7c6a5d1e1b9d") },
            new Product { Id = Guid.Parse("d0f1a2c3-e4f5-4a1b-b5a1-3c2e6b9e5f1d"), Name = "Product 5", Description = "Description of Product 5", Quantity = 30, Price = 49.99m, UserId = Guid.Parse("5a2d3c1b-7e8f-4f5b-9d2c-8b3a1c6e5d9f") },
            new Product { Id = Guid.Parse("e2d1a0c3-b5e6-4f4b-b3f2-1f4e6c3d1b8a"), Name = "Product 6", Description = "Description of Product 6", Quantity = 12, Price = 59.99m, UserId = Guid.Parse("6c3d2b1e-8f5a-4b2e-b6f5-7a3d1c6e1e9b") },
            new Product { Id = Guid.Parse("f3b1c2d4-e5f6-4c2e-9b1c-8f5a2d3e6b1b"), Name = "Product 7", Description = "Description of Product 7", Quantity = 8, Price = 69.99m, UserId = Guid.Parse("7b8c9d1e-3f2a-4e5f-b1d2-1c6e3b4e7a1a") },
            new Product { Id = Guid.Parse("c2f3d4e5-b6c7-4a1b-9f8d-2f4c5e1b6a2b"), Name = "Product 8", Description = "Description of Product 8", Quantity = 25, Price = 79.99m, UserId = Guid.Parse("8d1e2f3b-5c4a-4d1e-b6c5-3b2a1d7e9f8e") },
            new Product { Id = Guid.Parse("a3b2c1d4-e5f6-4f8b-b1d2-3e4c8f5a6d1b"), Name = "Product 9", Description = "Description of Product 9", Quantity = 18, Price = 89.99m, UserId = Guid.Parse("9c1b2d3e-4f5a-4b1c-6e8f-3d5a2b1f7e8e") },
            new Product { Id = Guid.Parse("b1c2d3f4-e5b6-4c2f-b1a3-8d5a1c6e2f9a"), Name = "Product 10", Description = "Description of Product 10", Quantity = 22, Price = 99.99m, UserId = Guid.Parse("0d1e2f7b-8f9a-4b5c-2e1d-6a3b4e5f1d9c") }
        ];

        public Product? GetByIdAsync(Guid id)
        {
            return products.FirstOrDefault(p => p.Id == id);
        }

        public void UpdateQuantity(Guid id, int quantity)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                throw new BadRequestException("");
            }

            if (product.Quantity < quantity)
            {
                throw new BadRequestException("");
            }

            product.Quantity -= quantity;
        }
    }
}
