using ChatsService.API.Authorization;
using ChatsService.API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Shared.Consts;
using Shared.Enums;
using System.Text;

namespace ChatsService.API.DI
{
    public static class DependencyInjection
    {
        public static void AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR(opt =>
            {
                opt.AddFilter<HubExceptionHandlingFilter>();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var jwtOptions = configuration.GetSection("JwtOptions");

                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions["SecretKey"]!))
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

                options.AddPolicy(Policies.WORKER, policy =>
                {
                    policy.AddRequirements(new RoleRequirment(RoleEnum.Admin, RoleEnum.Worker));
                });

                options.AddPolicy(Policies.USER, policy =>
                {
                    policy.AddRequirements(new RoleRequirment(RoleEnum.Admin, RoleEnum.Worker, RoleEnum.User));
                });
            });
        }
    }
}
