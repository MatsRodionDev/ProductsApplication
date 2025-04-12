using ChatsService.DAL.Interfaces;

namespace ChatsService.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void UseApplyMigrations(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices
                .CreateScope();

            var unitOfWork = scope.ServiceProvider
                .GetRequiredService<IUnitOfWork>();
    
            unitOfWork.MigrateDatabase();
        }
    }
}
