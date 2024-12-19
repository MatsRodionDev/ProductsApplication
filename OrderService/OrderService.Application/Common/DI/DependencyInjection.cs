using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common.Behaiviors;
using OrderService.Application.Common.Profiles;
using System.Reflection;

namespace OrderService.Application.Common.DI
{
    public static class DependencyInjection
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                cfg.AddOpenBehavior(typeof(ValidationPiplineBahavior<,>));
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<OrderFiltersProfile>();
                cfg.AddProfile<OrderProfile>();
                cfg.AddProfile<OrderItemProfile>();
                cfg.AddProfile<TakeProductDtoProfile>();
            });
        }
    }
}
