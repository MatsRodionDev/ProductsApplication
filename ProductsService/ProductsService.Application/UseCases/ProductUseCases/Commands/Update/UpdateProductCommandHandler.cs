using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Update
{
    public class UpdateProductCommandHandler(
        IProductCommandRepository repository,
        IEventBus eventBus,
        ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand>
    {
        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling UpdateProductCommand for ProductId: {ProductId}", request.Id);

            var product = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (product is null)
            {
                logger.LogWarning("Product with id {ProductId} not found", request.Id);
                throw new NotFoundException("Product with such id doesnt exist");
            }

            if (request.UserId != product.UserId)
            {
                logger.LogWarning("Unauthorized attempt to update product with id {ProductId} by UserId {UserId}", request.Id, request.UserId);
                throw new UnauthorizedException("User with such id cannot change this product");
            }

            product.Update(
                request.Name,
                request.Description,
                request.Quantity,
                request.Price);

            await repository.UpdateAsync(product, cancellationToken);
            await eventBus.PublishAsync(
                new ProductUpdatedEvent(product.Id, product.Quantity),
                cancellationToken);

            logger.LogInformation("Successfully updated Product with id {ProductId}", request.Id);
        }
    }
}
