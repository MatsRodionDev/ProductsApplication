using System.Threading.Tasks;
using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserEntity>
    {
        Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<List<UserEntity>> GetActivatedUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<List<UserEntity>> GetNotActivatedUsersAsync(int page = 1, int pageSize = 5, CancellationToken cancellationToken = default);
    }
}
