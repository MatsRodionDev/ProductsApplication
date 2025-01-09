using MediatR;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Interfaces.Services;

namespace ProductsService.Application.Common.Behaviors
{
    public class CacheInvalidationPipeline<TRequest, TResponse>(ICacheService cacheService)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICacheInvalidationCommand
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            await cacheService.RemoveAsync(
                request.Key,
                cancellationToken);

            return response;
        }
    }
}
