using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OprderService.Infrastructure.MessageBroker;
using OrderService.API.Authorization;
using Shared.Consts;
using Shared.Enums;
using System.Text;

namespace OrderService.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MessageBrokerSettings>(
                configuration.GetSection(nameof(MessageBrokerSettings)));
            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

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

            services.AddSingleton<IAuthorizationHandler, RoleRequirmentHandler>();
        }
    }
}
