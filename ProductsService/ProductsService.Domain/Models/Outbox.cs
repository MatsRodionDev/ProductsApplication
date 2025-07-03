using ProductsService.Domain.Abstrctions;

namespace ProductsService.Domain.Models
{
    public class Outbox : BaseModel
    {
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
