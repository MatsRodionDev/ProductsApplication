namespace ProductsService.Infrastructure.Services
{
    public class MinioOptions
    {
        public string Endpoint { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string Policy { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
