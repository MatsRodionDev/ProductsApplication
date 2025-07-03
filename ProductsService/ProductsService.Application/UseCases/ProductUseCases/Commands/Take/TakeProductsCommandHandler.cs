using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;
using System.Text;
using System.Data;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Take
{
    internal sealed class TakeProductsCommandHandler(
        IUnitOfWork unitOfWork) : ICommandHandler<TakeProductsCommand, List<TakedProductResponseDto>>
    {
        public async Task<List<TakedProductResponseDto>> Handle(TakeProductsCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

            try
            {
                var productsIds = request.Products.Select(p => p.Id).ToList();

                var products = await unitOfWork.Products.GetByIdsForUpdateAsync(productsIds, cancellationToken);

                var takedProducts = GetTakenProducts(request, products, out var errors);

                if (errors.Length > 0)
                {
                    throw new BadRequestException(errors.ToString());
                }

                unitOfWork.Products.UpdateMany(products);

                foreach (var product in products)
                {
                    await unitOfWork.Outboxes.AddAsync(
                        new ProductUpdatedEvent(product.Id, product.Quantity),
                        cancellationToken);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return takedProducts;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        }

        private List<TakedProductResponseDto> GetTakenProducts(TakeProductsCommand request, List<Product> products, out StringBuilder errors)
        {
            errors = new StringBuilder();

            List<TakedProductResponseDto> takedProducts = [];

            foreach (var requestProduct in request.Products)
            {
                var product = products.FirstOrDefault(p => p.Id == requestProduct.Id);

                if (product is null)
                {
                    errors.AppendLine($"Product with id {requestProduct.Id} doesn't exist.");
                    continue;
                }

                if (product.Quantity < requestProduct.Quantity)
                {
                    errors.AppendLine($"Product with id {requestProduct.Id} is not available in quantity {requestProduct.Quantity}.");
                    continue;
                }

                product.Quantity -= requestProduct.Quantity;
                takedProducts.Add(MapToTakenProductDto(product, requestProduct.Quantity));
            }

            return takedProducts;
        }

        private TakedProductResponseDto MapToTakenProductDto(Product product, int quantity)
        {
            return new TakedProductResponseDto(
                product.Id,
                product.Name,
                product.Price,
                quantity,
                product.UserId);
        }
    }
}
