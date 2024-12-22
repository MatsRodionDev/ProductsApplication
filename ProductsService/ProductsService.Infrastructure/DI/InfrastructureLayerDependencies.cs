using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using MongoDB.Driver;
using ProductsService.Application.Common;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Models;
using ProductsService.Infrastructure.MessageBroker;
using ProductsService.Infrastructure.MessageBroker.Consumers;
using ProductsService.Infrastructure.Services;
using ProductsService.Persistence.Interfaces;

namespace ProductsService.Infrastructure.DI
{
    public static class InfrastructureLayerDependencies
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<ProductCreatedEventConsumer>();
                busConfigurator.AddConsumer<ProductDeletedEventConsumer>();
                busConfigurator.AddConsumer<ProductUpdatedEventConsumer>();

                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();

                    configurator.Host(settings.Host, h =>
                    {
                        h.Username(settings.Username);
                        h.Password(settings.Password);
                        
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            var minioOptions = configuration.GetSection(nameof(MinioOptions));

            services.AddSingleton(new MinioClient()
                .WithEndpoint(minioOptions["Endpoint"])
                .WithCredentials(minioOptions["AccessKey"], minioOptions["SecretKey"])
                .Build());

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ICacheService, CacheService>();

            services.AddTransient<IEventBus, EventBus>();
            
        }

        
    }
}

