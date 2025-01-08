using ChatsService.BLL.Models;

namespace ChatsService.BLL.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
