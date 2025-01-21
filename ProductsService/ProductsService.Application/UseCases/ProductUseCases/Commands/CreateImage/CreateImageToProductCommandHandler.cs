using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.CreateImage
{
    public class CreateImageToProductCommandHandler(
        IProductCommandRepository repository,
        IFileService fileService,
        IEventBus eventBus,
        ILogger<CreateImageToProductCommandHandler> logger) : ICommandHandler<CreateImageToProductCommand>
    {
        public async Task Handle(CreateImageToProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting to handle CreateImageToProductCommand for ProductId: {ProductId}", request.ProductId);

            var product = await repository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                logger.LogError("Product with id {ProductId} not found", request.ProductId);
                throw new NotFoundException("Product with such id doesn't exist");
            }

            if (product.UserId != request.UserId)
            {
                logger.LogError("User with id {UserId} is not authorized to change Product with id {ProductId}", request.UserId, request.ProductId);
                throw new UnauthorizedException("User with such id cannot change this product");
            }

            var imageName = await fileService.UploadFileAsync(request.File, cancellationToken);
            var image = new Image
            {
                ImageName = imageName
            };

            product.Images.Add(image);

            await repository.UpdateAsync(product, cancellationToken);
            await eventBus.PublishAsync(
                new ProductUpdatedEvent(product.Id, product.Quantity),
                cancellationToken);

            logger.LogInformation("Product with id {ProductId} successfully updated with new image", request.ProductId);
        }
    }
}
