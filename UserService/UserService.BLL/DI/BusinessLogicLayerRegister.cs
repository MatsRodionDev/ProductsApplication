using Hangfire;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.BLL.Common.Hashers;
using UserService.BLL.Common.MessageBroker;
using UserService.BLL.Interfaces.Hashers;
using UserService.BLL.Interfaces.Jobs;
using UserService.BLL.Interfaces.MessageBroker;
using UserService.BLL.Interfaces.Services;
using UserService.BLL.Jobs;
using UserService.BLL.Profiles;
using UserService.BLL.Services;


namespace UserService.BLL.DI
{
    public static class BusinessLogicLayerRegister
    {
        public static void RegisteBusinessLogicLayerDapendencies(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            services.AddHangfire(cfg =>
            {
                var connectionString = configuration.GetConnectionString("ApplicationDbContext");

                cfg.UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(connectionString);
            });
            services.AddHangfireServer();

            services.AddMassTransit(busConfigurator =>
            {
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

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddTransient<IEventBus, EventBus>();
            services.AddTransient<IUserJobsService, UserJobsService>();

            services.AddAutoMapper(typeof(UserProfile));
        }
    }
}
