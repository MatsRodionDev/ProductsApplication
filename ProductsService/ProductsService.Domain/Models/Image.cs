namespace ProductsService.Domain.Models
{
    public class Image
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ImageName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
