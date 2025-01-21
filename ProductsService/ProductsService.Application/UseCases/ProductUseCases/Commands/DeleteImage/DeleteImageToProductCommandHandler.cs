using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteImage
{
    public class DeleteImageToProductCommandHandler(
        IProductCommandRepository repository,
        IFileService fileService,
        IEventBus eventBus,
        ILogger<DeleteImageToProductCommandHandler> logger) : ICommandHandler<DeleteImageToProductCommand>
    {
        public async Task Handle(DeleteImageToProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling DeleteImageToProductCommand for ProductId: {ProductId}, ImageId: {ImageId}", request.ProductId, request.ImageId);

            var product = await repository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                logger.LogError("Product with id {ProductId} not found", request.ProductId);
                throw new NotFoundException("Product with such id doesn't exist");
            }

            if (product.UserId != request.UserId)
            {
                logger.LogError("Unauthorized attempt to modify ProductId: {ProductId} by UserId: {UserId}", request.ProductId, request.UserId);
                throw new UnauthorizedException("User with such id cannot change this product");
            }

            var image = product.Images.Find(i => i.Id == request.ImageId);

            if (image is null)
            {
                logger.LogError("Image with id {ImageId} not found in ProductId: {ProductId}", request.ImageId, request.ProductId);
                throw new NotFoundException("Image with such id doesn't exist");
            }

            product.Images.Remove(image);

            await repository.UpdateAsync(product, cancellationToken);
            await fileService.RemoveFileAsync(image.ImageName, cancellationToken);

            await eventBus.PublishAsync(
                new ProductUpdatedEvent(product.Id, product.Quantity),
                cancellationToken);

            logger.LogInformation("Image with id {ImageId} removed from ProductId: {ProductId} and event published", request.ImageId, request.ProductId);
        }
    }
}
