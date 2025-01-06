using Microsoft.EntityFrameworkCore;
using UserService.DAL.Interfaces;

namespace UserService.DAL.UoW
{
    public class UnitOfWork(
        ApplicationDbContext context,
        IUserRepository userRepository,
        IRoleRepository roleRepository) : IUnitOfWork
    {
        public IRoleRepository RoleRepository => roleRepository;
        public IUserRepository UserRepository => userRepository;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

        public void DatabaseMigrate()
        {
            if (context.Database.IsRelational())
            {
                context.Database.Migrate();
            }
        }
    }
}
