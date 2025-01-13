using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using UserService.BLL.Common.Hashers;
using UserService.BLL.Interfaces.Hashers;
using UserService.BLL.Interfaces.Jobs;
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

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddTransient<IUserJobsService, UserJobsService>();

            services.AddAutoMapper(typeof(UserProfile));
        }
    }
}
