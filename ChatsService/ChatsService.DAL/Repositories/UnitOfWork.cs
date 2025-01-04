using ChatsService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatsService.DAL.Repositories
{
    public class UnitOfWork(
        ApplicationDbContext context,
        IMessageRepository messageRepository,
        IChatRepository chatRepository) : IUnitOfWork
    {
        public IChatRepository ChatRepository => chatRepository;
        public IMessageRepository MessageRepository => messageRepository;

        public void MigrateDatabase()
        {
            if(context.Database.IsRelational())
            {
                context.Database.Migrate();
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
