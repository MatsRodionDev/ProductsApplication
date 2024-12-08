using Microsoft.Extensions.Options;
using Minio.DataModel.Args;
using Minio;
using ProductsService.Application.Common;
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

        public async Task UploadFileAsync(string objName, IFormFile file, CancellationToken cancellationToken = default)
        {
            await CreateBucketAsync();

            var stream = file.OpenReadStream();

            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_minioOptions.BucketName)
                .WithObject(objName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("application/octet-stream"), cancellationToken);
        }

        private async Task<bool> BucketExistsAsync()
        {
            return await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_minioOptions.BucketName));
        }

        private async Task CreateBucketAsync()
        {
            if (!await BucketExistsAsync())
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs()
                    .WithBucket(_minioOptions.BucketName));

                var policyJson = $@"{{
                      ""Version"": ""2012-10-17"",
                      ""Statement"": [
                        {{
                          ""Effect"": ""Allow"",
                          ""Principal"": {{ ""AWS"": [""*""] }},
                          ""Action"": [""s3:GetBucketLocation"", ""s3:ListBucket"", ""s3:GetObject""],
                          ""Resource"": [
                            ""arn:aws:s3:::{_minioOptions.BucketName}"",
                            ""arn:aws:s3:::{_minioOptions.BucketName}/*""
                          ]
                        }}
                      ]
                    }}";

                var args = new SetPolicyArgs()
                    .WithBucket(_minioOptions.BucketName)
                    .WithPolicy(policyJson);
                await _minioClient.SetPolicyAsync(args);
            }
        }
    }
}
