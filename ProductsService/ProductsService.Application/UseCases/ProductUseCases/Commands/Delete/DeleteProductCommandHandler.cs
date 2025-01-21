using ProductsService.Application.Common.Abstractions;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using ProductsService.Application.Common.Interfaces.Services;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Delete
{
    public class DeleteProductCommandHandler(
        IProductCommandRepository repository,
        IEventBus eventBus,
        IFileService fileService,
        ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand>
    {
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting to handle DeleteProductCommand for ProductId: {ProductId}", request.ProductId);

            var product = await repository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                logger.LogError("Product with id {ProductId} not found", request.ProductId);
                throw new NotFoundException("Product with such id doesn't exist");
            }

            if (product.UserId != request.UserId)
            {
                logger.LogError("User with id {UserId} is not authorized to delete Product with id {ProductId}", request.UserId, request.ProductId);
                throw new UnauthorizedException("User with such id cannot delete this product");
            }

            foreach (var image in product.Images)
            {
                logger.LogInformation("Removing image {ImageName} from ProductId: {ProductId}", image.ImageName, request.ProductId);
                await fileService.RemoveFileAsync(image.ImageName, cancellationToken);
            }

            await repository.RemoveAsync(request.ProductId, cancellationToken);
            await eventBus.PublishAsync(
                new ProductDeletedEvent(request.ProductId),
                cancellationToken);

            logger.LogInformation("Product with id {ProductId} successfully removed", request.ProductId);
        }
    }
}
