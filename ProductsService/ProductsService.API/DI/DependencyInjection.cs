using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductsService.API.Interceptors;
using ProductsService.API.Profiles;
using ProductsService.Infrastructure.MessageBroker;
using ProductsService.Infrastructure.Services;
using ProductsService.Persistence.Settings;
using Shared.Consts;
using System.Text;

namespace ProductsService.API.DI
{
    public static class DependencyInjection
    {
        public static void AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MinioOptions>(configuration.GetSection(nameof(MinioOptions)));

            services.Configure<CommandMongoDbSettings>(configuration.GetSection(nameof(CommandMongoDbSettings)));
            services.Configure<QueryMongoDbSettings>(configuration.GetSection(nameof(QueryMongoDbSettings)));

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

            services.Configure<MessageBrokerSettings>(
                configuration.GetSection(nameof(MessageBrokerSettings)));

            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

            services.AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionHandlingInterceptor>();
            });
            services.AddGrpcReflection();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ImageGrpcProfile>();
                cfg.AddProfile<ProductGrpcProfile>();
            });
        }
    }
}
