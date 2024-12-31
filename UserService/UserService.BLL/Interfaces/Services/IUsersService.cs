using UserService.BLL.Models;

namespace UserService.BLL.Interfaces.Services
{
    public interface IUsersService 
    {
        Task UpdateAsyc(Guid userId, string firstName, string lastName, CancellationToken cancellationToken = default);
        Task<List<User>> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task UpdateRoleToUser(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
    }
}
