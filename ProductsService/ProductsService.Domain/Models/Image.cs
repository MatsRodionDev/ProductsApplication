using ProductsService.Domain.Abstrctions;

namespace ProductsService.Domain.Models
{
    public class Image : BaseModel
    {
        public Guid UserId { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
