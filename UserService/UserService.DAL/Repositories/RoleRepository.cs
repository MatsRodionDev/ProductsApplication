using Microsoft.EntityFrameworkCore;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class RoleRepository(ApplicationDbContext context)
        : GenericRepository<RoleEntity>(context),
        IRoleRepository
    {
        public async Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
        }
    }
}
