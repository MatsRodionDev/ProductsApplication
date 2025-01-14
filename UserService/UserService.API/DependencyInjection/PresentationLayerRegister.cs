using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Shared.Consts;
using Shared.Enums;
using System.Text;
using UserService.API.Authorization;
using UserService.API.Dtos.Requests;
using UserService.API.Filters;
using UserService.API.Profiles;
using UserService.BLL.Common;

namespace UserService.API.DependencyInjection
{
    public static class PresentationLayerRegister
    {
        public static void AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
            services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));

            services.AddControllers(options => options.Filters
                .Add(typeof(ValidationFilter)));

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<LoginUserRequest>();

            services.AddAutoMapper(typeof(UserProfile));

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

            services.AddSingleton<IAuthorizationHandler, RoleRequirmentHandler>();
        }
    }
}
