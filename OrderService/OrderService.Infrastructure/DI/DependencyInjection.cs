using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common.Intefaces;
using OrderService.Infrastructure.Grpc;

namespace OrderService.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddSingleton<IProductService, ProductService>();
        }
    }
}
