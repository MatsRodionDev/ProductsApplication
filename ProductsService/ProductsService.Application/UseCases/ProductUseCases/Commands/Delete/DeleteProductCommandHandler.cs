using ProductsService.Application.Common.Abstractions;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using ProductsService.Application.Common.Interfaces.Services;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Delete
{
    public class DeleteProductCommandHandler(
        IUnitOfWork unitOfWork) : ICommandHandler<DeleteProductCommand>
    {
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product with such id doesnt exist");
            }

            if (product.UserId != request.UserId)
            {
                throw new UnauthorizedException("User with such id cannot delete this product");
            }

            unitOfWork.Products.Remove(product);
            await unitOfWork.Outboxes.AddAsync(new ProductCreatedEvent(product.Id), cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
