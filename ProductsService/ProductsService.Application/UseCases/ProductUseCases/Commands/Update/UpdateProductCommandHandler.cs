using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using System.Data;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Update
{
    public class UpdateProductCommandHandler(
        IUnitOfWork unitOfWork): ICommandHandler<UpdateProductCommand>
    {
        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

            try
            {
                var product = await unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);

                if (product is null)
                {
                    throw new NotFoundException("Product with such id doesnt exist");
                }

                if (request.UserId != product.UserId)
                {
                    throw new UnauthorizedException("User with such id cannot change this product");
                }

                if (request.Quantity + product.Quantity < 0)
                {
                    throw new BadRequestException("There is no product with this id in this quantity");
                }

                product.Update(
                    request.Name,
                    request.Description,
                    request.Quantity + product.Quantity,
                    request.Price);

                unitOfWork.Products.Update(product);
                await unitOfWork.Outboxes.AddAsync(
                    new ProductUpdatedEvent(product.Id, product.Quantity),
                    cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

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
