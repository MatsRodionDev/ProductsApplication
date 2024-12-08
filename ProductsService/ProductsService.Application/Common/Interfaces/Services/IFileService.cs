using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.Common.Interfaces.Services
{
    public interface IFileService
    {
        Task UploadFileAsync(string objName, IFormFile file, CancellationToken cancellationToken = default);
        Task RemoveFileAsync(string objName, CancellationToken cancellationToken = default);
    }
}
