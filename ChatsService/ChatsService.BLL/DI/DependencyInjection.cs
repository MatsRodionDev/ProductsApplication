using ChatsService.BLL.Interfaces;
using ChatsService.BLL.Profiles;
using ChatsService.BLL.Protos;
using ChatsService.BLL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatsService.BLL.DI
{
    public static class DependencyInjection
    {
        public static void AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<Products.ProductsClient>(opt =>
            {
                opt.Address = new Uri(configuration["gRPC:ServerUrl"]!);
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ChatProfile>();
                cfg.AddProfile<MessageProfile>();
                cfg.AddProfile<ChatResponseProfile>();
            });

            services
                .AddScoped<ICacheService, CacheService>()
                .AddScoped<IProductService, ProductsService>()
                .AddScoped<IChatService, ChatService>();
        }
    }
}
