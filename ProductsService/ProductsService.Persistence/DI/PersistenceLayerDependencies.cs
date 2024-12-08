using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.Persistence.Interfaces;
using ProductsService.Persistence.Persistence;
using ProductsService.Persistence.Repositories;
using ProductsService.Domain.Interfaces;
using MongoDB.Driver;
using ProductsService.Domain.Models;

namespace ProductsService.Persistence.DI
{
    public static class PersistenceLayerDependencies
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMongoCommandContext, MongoCommandContext>();
            services.AddScoped<IMongoQueryContext, MongoQueryContext>();

            services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
            services.AddScoped<IProductCommandRepository, ProductCommandRepository>();

            MongoDbPersistence.Configure();
        }

        
    }
}
