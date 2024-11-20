using Microsoft.EntityFrameworkCore;
using UserService.DAL;

namespace UserService.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext context = scope
                .ServiceProvider.GetService<ApplicationDbContext>()!;

            context.Database.Migrate();
        }
    }
}
