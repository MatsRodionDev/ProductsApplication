using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Consts;
using Shared.Enums;
using System.Text;
using UserService.BLL.Common;
using UserService.BLL.Common.Auth;
using UserService.BLL.Common.Hashers;
using UserService.BLL.Interfaces.Hashers;
using UserService.BLL.Interfaces.Services;
using UserService.BLL.Profiles;
using UserService.BLL.Services;


namespace UserService.BLL.DI
{
    public static class BusinessLogicLayerRegister
    {
        public static void RegisteBusinessLogicLayerDapendencies(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[CookiesConstants.ACCESS];

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ADMIN, policy =>
                {
                    policy.AddRequirements(new RoleRequirment(RoleEnum.Admin));
                });


                options.AddPolicy(Policies.USER, policy =>
                {
                    policy.AddRequirements(new RoleRequirment(RoleEnum.User));
                });
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            services.AddSingleton<IAuthorizationHandler, RoleRequirmentHandler>();

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddAutoMapper(typeof(BusinessLogicLayerProfile));
        }
    }
}
