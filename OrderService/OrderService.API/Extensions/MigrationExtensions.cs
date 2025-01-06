using OrderService.Domain.Interfaces;

namespace OrderService.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void useApplyMigrations(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            unitOfWork.DataBaseMigrate();
        }
    }
}
