using MediatR;

namespace ProductsService.Application.Common.Abstractions
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }

    public interface IQuery : IRequest
    {
    }
}
