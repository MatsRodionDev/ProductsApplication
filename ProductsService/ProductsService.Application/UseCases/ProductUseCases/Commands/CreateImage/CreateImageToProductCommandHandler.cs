using MediatR;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Events.Product;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.CreateImage
{
    public class CreateImageToProductCommandHandler(
        IProductCommandRepository repository,
        IFileService fileService,
        IMediator mediator) : ICommandHandler<CreateImageToProductCommand>
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
                ImageName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}"
            };

            product.Images.Add(image);

            await fileService.UploadFileAsync(image.ImageName, request.File, cancellationToken);

            await repository.UpdateAsync(product, cancellationToken);
            await mediator.Publish(
                new ProductUpdatedEvent(product.Id),
                cancellationToken);
        }
    }
}
