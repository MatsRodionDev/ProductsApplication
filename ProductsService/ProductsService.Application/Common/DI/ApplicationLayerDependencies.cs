using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.Application.Common.Behaviors;
using ProductsService.Application.Common.Profiles;
using ProductsService.Domain.Interfaces;
using System.Reflection;

namespace ProductsService.Application.Common.DI
{
    public static class ApplicationLayerDependencies
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
                cfg.AddOpenBehavior(typeof(QueryCachingPipelineBehavior<,>));
                cfg.AddOpenBehavior(typeof(CacheInvalidationPipeline<,>));  
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CategoryProfile>();
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<ProductResponseProfile>();
                cfg.AddProfile<ImageProfile>();
                cfg.AddProfile<UsersProductsFilterProfile>();
                cfg.AddProfile<ProductsFiltersProfile>();
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
