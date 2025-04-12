using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.CreateImage
{
    public class CreateImageToProductCommandHandler(
        IProductCommandRepository repository,
        IFileService fileService,
        IEventBus eventBus) : ICommandHandler<CreateImageToProductCommand>
    {
        public async Task Handle(CreateImageToProductCommand request, CancellationToken cancellationToken)
        {
            var product = await repository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product with such id doesnt exist");
            }

            if(product.UserId != request.UserId)
            {
                throw new UnauthorizedException("User with such id cannot change this product");
            }

            var image = new Image
            {
                ImageName = await fileService.UploadFileAsync(request.File, cancellationToken)
            };
            product.Images.Add(image);

            await repository.UpdateAsync(product, cancellationToken);
            await eventBus.PublishAsync(
                new ProductUpdatedEvent(product.Id, product.Quantity),
                cancellationToken);
        }
    }
}
