using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.DAL.DataBase
{
    public static class DatabaseMigrations
    {
        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();
        }
    }
}
