using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.TakeOne
{
    internal sealed class TakeOneProductCommandHandler(
        IProductCommandRepository commandRepository,
        IEventBus eventBus,
        ILogger<TakeOneProductCommandHandler> logger) : ICommandHandler<TakeOneProductCommand, TakedProductResponseDto>
    {
        public async Task<TakedProductResponseDto> Handle(TakeOneProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling TakeOneProductCommand for ProductId: {ProductId}", request.Id);

            var product = await commandRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product is null)
            {
                logger.LogWarning("Product with id {ProductId} not found", request.Id);
                throw new NotFoundException($"Product with id {request.Id} doesnt exist");
            }

            if (product.Quantity < request.Quantity)
            {
                logger.LogWarning("Product with id {ProductId} does not have enough quantity", request.Id);
                throw new BadRequestException($"Product with id {request.Id} doesnt available in this quantity");
            }

            product.Quantity -= request.Quantity;

            await commandRepository.UpdateAsync(product);

            await eventBus.PublishAsync(
                new ProductUpdatedEvent(product.Id, product.Quantity),
                cancellationToken);

            var takedProductDto = new TakedProductResponseDto(
                product.Id,
                product.Name,
                product.Price,
                request.Quantity,
                product.UserId);

            logger.LogInformation("Successfully processed TakeOneProductCommand for ProductId: {ProductId}, Quantity Taken: {Quantity}", request.Id, request.Quantity);

            return takedProductDto;
        }
    }
}
