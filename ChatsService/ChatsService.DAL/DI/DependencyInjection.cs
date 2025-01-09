using ChatsService.DAL.Interfaces;
using ChatsService.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatsService.DAL.DI
{
    public static class DependencyInjection
    {
        public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options
                => options.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDbContext))));

            services
                .AddScoped<IChatRepository, ChatRepository>()
                .AddScoped<IMessageRepository, MessageRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
