using MediatR;

namespace ProductsService.Application.Common.Abstractions
{
    public interface ICommand<TResponse> : IRequest<TResponse>
    {
    }

    public interface ICommand : IRequest
    {
    }
}
