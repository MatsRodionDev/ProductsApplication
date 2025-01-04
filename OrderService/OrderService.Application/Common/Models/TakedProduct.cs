using System;

namespace OrderService.Application.Common.Models
{
    public class TakedProduct
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int TakedQuantity { get; set; }
        public Guid UserId { get; set; }
    }
}
