﻿using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteImage
{
    public class DeleteImageToProductCommandHandler(
        IProductCommandRepository repository,
        IFileService fileService,
        IEventBus eventBus) : ICommandHandler<DeleteImageToProductCommand>
    {
        public async Task Handle(DeleteImageToProductCommand request, CancellationToken cancellationToken)
        {
            var product = await repository.GetByIdAsync(request.ProductId, cancellationToken);

            if(product is null)
            {
                throw new NotFoundException("Product with such id doesnt exist");
            }

            if (product.UserId != request.UserId)
            {
                throw new UnauthorizedException("User with such id cannot change this product");
            }

            var image = product.Images
                .Find(i => i.Id == request.ImageId);

            if (image is null)
            {
                throw new NotFoundException("Image with such id doesnt exist");
            }

            product.Images.Remove(image);

            await repository.UpdateAsync(product, cancellationToken);
            await fileService.RemoveFileAsync(image.ImageName, cancellationToken);

            await eventBus.PublishAsync(
                new ProductUpdatedEvent(product.Id, product.Quantity), 
                cancellationToken);
        }
    }
}
