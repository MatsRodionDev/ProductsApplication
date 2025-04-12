using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.Common.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default);
        Task RemoveFileAsync(string objName, CancellationToken cancellationToken = default);
        string GetFileUrl(string fileName);
    }
}
