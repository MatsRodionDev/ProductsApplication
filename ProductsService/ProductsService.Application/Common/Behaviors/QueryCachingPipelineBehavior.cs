using MediatR;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Interfaces.Services;

namespace ProductsService.Application.Common.Behaviors
{
    internal sealed class QueryCachingPipelineBehavior<TRequest, TResponse>(ICacheService cacheService)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedQuery
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            var response = await cacheService.GetAsync<TResponse>(
                request.Key,
                cancellationToken);

            if (response is not null)
            {
                return response;
            }

            response = await next();

            await cacheService.SetAsync(
                request.Key, 
                response,
                request.SlidingExpiration,
                request.AbsoluteExpiration, 
                cancellationToken);

            return response;
        }
    }
}
