using ProductsService.Domain.Abstrctions;

namespace ProductsService.Domain.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; } = string.Empty;
    }
}
