using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Domain.Interfaces;
using OrderService.Persistence.Repositories;
using OrderService.Persistence.UoW;

namespace OrderService.Persistence.DI
{
    public static class DependencyInjection
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDbContext))));

            services
                .AddScoped<IBasketRepository, BasketRepository>()
                .AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped<IBasketItemRepository, BasketItemRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>();

            var serviceProvider = services.BuildServiceProvider();

            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();
        }
    }
}
