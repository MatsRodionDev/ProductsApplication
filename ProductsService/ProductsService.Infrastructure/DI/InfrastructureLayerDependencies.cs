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
            //var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            //    {
            //        options.TokenValidationParameters = new()
            //        {
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
            //        };

            //        options.Events = new JwtBearerEvents()
            //        {
            //            OnMessageReceived = context =>
            //            {
            //                context.Token = context.Request.Cookies[CookiesConstants.ACCESS];

            //                return Task.CompletedTask;
            //            }
            //        };
            //    });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(Policies.ADMIN, policy =>
            //    {
            //        policy.AddRequirements(new RoleRequirment(RoleEnum.Admin));
            //    });


            //    options.AddPolicy(Policies.USER, policy =>
            //    {
            //        policy.AddRequirements(new RoleRequirment(RoleEnum.User));
            //    });
            //});

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

            //services.AddSingleton<IAuthorizationHandler, RoleRequirmentHandler>();

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

