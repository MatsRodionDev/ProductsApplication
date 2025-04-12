using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;
using System.Text;
using System.Threading;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Take
{
    internal sealed class TakeProductsCommandHandler(
        IProductCommandRepository commandRepository,
        IEventBus eventBus) : ICommandHandler<TakeProductsCommand, List<TakedProductResponseDto>>
    {
        public async Task<List<TakedProductResponseDto>> Handle(TakeProductsCommand request, CancellationToken cancellationToken)
        {
            var errors = new StringBuilder();

            List<Product> products = [];
            List<TakedProductResponseDto> takedProducts = [];

            foreach (var productToTake in request.Products)
            {
                var product = await commandRepository.GetByIdAsync(productToTake.Id, cancellationToken);

                if (product is null)
                {
                    errors.Append($"Product with id {productToTake.Id} doesnt available");
                    continue;
                }

                if (product.Quantity < productToTake.Quantity)
                {
                    errors.Append($"Product with id {productToTake.Id} doesnt available in this quantity");
                    continue;
                }

                product.Quantity -= productToTake.Quantity;

                products.Add(product);
                takedProducts.Add(
                    CreateTakedProductDto(product, productToTake.Quantity)); 
            }

            if (errors.Length > 0)
            {
                throw new BadRequestException(errors.ToString());
            }

            await commandRepository.UpdateManyAsync(products, cancellationToken);
            await PublishEventsAsync(products, cancellationToken);

            return takedProducts;
        }

        private TakedProductResponseDto CreateTakedProductDto(Product product, int quantity)
        {
            return new TakedProductResponseDto(
                product.Id,
                product.Name,
                product.Price,
                quantity,
                product.UserId);
        }

        private async Task PublishEventsAsync(List<Product> products, CancellationToken cancellationToken = default)
        {
            var publishTasks = products
                .Select(p => eventBus.PublishAsync(
                    new ProductUpdatedEvent(p.Id, p.Quantity),
                    cancellationToken));

            await Task.WhenAll(publishTasks);
        }

    }
}
