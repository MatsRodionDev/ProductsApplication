using Microsoft.EntityFrameworkCore;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class UserRepository(ApplicationDbContext context)
        : GenericRepository<UserEntity>(context),
        IUserRepository
    {
        public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public override async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<UserEntity>> GetActivatedUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(u => u.IsActivated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserEntity>> GetNotActivatedUsersAsync(int page = 1, int pageSize = 5, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(u => u.IsActivated == false)
                .Where(u => u.CreatedAt.AddMinutes(1) <= DateTime.UtcNow)
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
