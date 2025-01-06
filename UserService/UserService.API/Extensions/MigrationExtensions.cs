using System.Runtime.CompilerServices;
using UserService.DAL.Interfaces;

namespace UserService.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void UseApplyMigrations(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            unitOfWork.DatabaseMigrate();
        }
    }
}
