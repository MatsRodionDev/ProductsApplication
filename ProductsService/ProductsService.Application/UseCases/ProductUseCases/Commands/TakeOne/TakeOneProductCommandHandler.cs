using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using System.Data;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.TakeOne
{
    internal sealed class TakeOneProductCommandHandler(
        IUnitOfWork unitOfWork) : ICommandHandler<TakeOneProductCommand, TakedProductResponseDto>
    {
        public async Task<TakedProductResponseDto> Handle(TakeOneProductCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

            try
            {
                var product = await unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);

                if (product is null)
                {
                    throw new NotFoundException($"Product with id {request.Id} doesnt exist");
                }

                if (product.Quantity < request.Quantity)
                {
                    throw new BadRequestException($"Product with id {request.Id} doesnt available in this quantity");
                }

                product.Quantity -= request.Quantity;

                unitOfWork.Products.Update(product);

                await unitOfWork.Outboxes.AddAsync(
                    new ProductUpdatedEvent(product.Id, product.Quantity),
                    cancellationToken);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new TakedProductResponseDto(
                   product.Id,
                   product.Name,
                   product.Price,
                   request.Quantity,
                   product.UserId);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        }
    }
}
