using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteImage
{
    public class DeleteImageToProductCommandHandler(
        IUnitOfWork unitOfWork,
        IFileService fileService) : ICommandHandler<DeleteImageToProductCommand>
    {
        public async Task Handle(DeleteImageToProductCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);

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

            await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

            try
            {
                product.Images.Remove(image);

                unitOfWork.Products.Update(product);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                await fileService.RemoveFileAsync(image.ImageName, cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch 
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        }
    }
}
