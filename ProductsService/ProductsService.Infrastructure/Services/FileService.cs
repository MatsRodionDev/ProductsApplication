using Microsoft.Extensions.Options;
using Minio.DataModel.Args;
using Minio;
using Microsoft.AspNetCore.Http;
using ProductsService.Application.Common.Interfaces.Services;

namespace ProductsService.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IMinioClient _minioClient;
        private readonly MinioOptions _minioOptions;

        public FileService(IMinioClient minioClient, IOptions<MinioOptions> minioOptions)
        {
            _minioOptions = minioOptions.Value;
            _minioClient = minioClient;
        }

        public async Task RemoveFileAsync(string objName, CancellationToken cancellationToken = default)
        {
            await _minioClient.RemoveObjectAsync(
                new RemoveObjectArgs()
                .WithBucket(_minioOptions.BucketName)
                .WithObject(objName), 
                cancellationToken);
        }

        public async Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            await CreateBucketAsync();

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var stream = file.OpenReadStream();

            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_minioOptions.BucketName)
                .WithObject(fileName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("application/octet-stream"), cancellationToken);

            return fileName;
        }

        public string GetFileUrl(string fileName)
        {
            return $"http://{_minioOptions.ImageUrl}/{_minioOptions.BucketName}/{fileName}";
        }

        private async Task<bool> BucketExistsAsync()
        {
            return await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_minioOptions.BucketName));
        }

        private async Task CreateBucketAsync()
        {
            if (await BucketExistsAsync())
            {
                return;
            }

            await _minioClient.MakeBucketAsync(new MakeBucketArgs()
                    .WithBucket(_minioOptions.BucketName));

            var policyJson = _minioOptions.Policy;

            var args = new SetPolicyArgs()
                .WithBucket(_minioOptions.BucketName)
                .WithPolicy(policyJson);
            await _minioClient.SetPolicyAsync(args);
        }
    }
}
