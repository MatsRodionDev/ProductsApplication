using ProductsService.Application.Common.Abstractions;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using ProductsService.Application.Common.Interfaces.Services;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Delete
{
    public class DeleteProductCommandHandler(
        IProductCommandRepository repository,
        IEventBus eventBus,
        IFileService fileService) : ICommandHandler<DeleteProductCommand>
    {
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await repository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product with such id doesnt exist");
            }

            if (product.UserId != request.UserId)
            {
                throw new UnauthorizedException("User with such id cannot delete this product");
            }

            foreach (var image in product.Images)
            {
                await fileService.RemoveFileAsync(image.ImageName, cancellationToken);
            }

            await repository.RemoveAsync(request.ProductId, cancellationToken);
            await eventBus.PublishAsync(
                new ProductDeletedEvent(request.ProductId), 
                cancellationToken);
        }
    }
}
