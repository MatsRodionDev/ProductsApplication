using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common.Intefaces;
using OrderService.Infrastructure.Grpc;
using OrderService.Infrastructure.Profiles;
using ProductsService.API.Protos;

namespace OrderService.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductService, ProductService>();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ImageProfile>();
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<TakedProductProfile>();
                cfg.AddProfile<ProductRequestProfile>();
            });

            services
                .AddGrpcClient<Products.ProductsClient>(opt =>
                {
                    opt.Address = new Uri(configuration["gRPC:ServerUrl"]!);
                });
        }
    }
}
