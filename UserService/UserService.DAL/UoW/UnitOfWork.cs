using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UserService.DAL.Abstractions;
using UserService.DAL.Interfaces;
using UserService.DAL.Outbox;

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
            var messages = context.ChangeTracker
                .Entries<BaseEntity>()
                .Select(x => x.Entity)
                .SelectMany(entity =>
                {
                    var events = entity.GetEvents();

                    entity.ClearEvents();

                    return events;
                })
                .Select(integrationEvent => new OutboxMessageEntity
                {
                    Id = Guid.NewGuid(),
                    OccuredOnUtc = DateTime.UtcNow,
                    Type = integrationEvent.GetType().ToString(),
                    Content = JsonConvert.SerializeObject(
                        integrationEvent, 
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        })
                })
                .ToList();

            await context.Outboxes.AddRangeAsync(messages, cancellationToken);

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
