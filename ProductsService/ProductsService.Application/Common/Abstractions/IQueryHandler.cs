using MediatR;

namespace ProductsService.Application.Common.Abstractions
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
    }

    public interface IQueryHandler<TQuery> : IRequestHandler<TQuery>
        where TQuery : IQuery
    {
    }
}
