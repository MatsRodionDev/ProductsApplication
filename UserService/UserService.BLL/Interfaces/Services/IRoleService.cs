using UserService.BLL.Models;

namespace UserService.BLL.Interfaces.Services
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    }
}
