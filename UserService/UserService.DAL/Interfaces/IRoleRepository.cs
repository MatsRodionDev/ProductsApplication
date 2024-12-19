using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IRoleRepository : IGenericRepository<RoleEntity>
    {
        Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
