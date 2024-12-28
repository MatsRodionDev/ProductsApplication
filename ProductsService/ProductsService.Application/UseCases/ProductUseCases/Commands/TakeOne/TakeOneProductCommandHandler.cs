using MediatR;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Application.Common.Events.Product;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.TakeOne
{
    internal sealed class TakeOneProductCommandHandler(
        IProductCommandRepository commandRepository,
        IMediator mediator) : ICommandHandler<TakeOneProductCommand, TakedProductResponseDto>
    {
        public async Task<TakedProductResponseDto> Handle(TakeOneProductCommand request, CancellationToken cancellationToken)
        {
            var product = await commandRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException($"Product with id {request.Id} doesnt exist");
            }

            if (product.Quantity < request.Quantity)
            {
                throw new BadRequestException($"Product with id {request.Id} doesnt available in this quantity");
            }

            product.Quantity -= request.Quantity;

            await commandRepository.UpdateAsync(product);

            await mediator.Publish(new ProductUpdatedEvent(product.Id), cancellationToken);

            return new TakedProductResponseDto(
                product.Id,
                product.Name,
                product.Price,
                request.Quantity,
                product.UserId);
        }
    }
}
