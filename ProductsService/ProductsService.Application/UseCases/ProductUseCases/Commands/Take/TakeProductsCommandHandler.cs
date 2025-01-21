using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Take
{
    internal sealed class TakeProductsCommandHandler(
        IProductCommandRepository commandRepository,
        IEventBus eventBus,
        ILogger<TakeProductsCommandHandler> logger) : ICommandHandler<TakeProductsCommand, List<TakedProductResponseDto>>
    {
        public async Task<List<TakedProductResponseDto>> Handle(TakeProductsCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling TakeProductsCommand");

            var errors = new StringBuilder();
            List<Product> products = new List<Product>();
            List<TakedProductResponseDto> takedProducts = new List<TakedProductResponseDto>();

            foreach (var productToTake in request.Products)
            {
                var product = await commandRepository.GetByIdAsync(productToTake.Id, cancellationToken);

                if (product is null)
                {
                    errors.Append($"Product with id {productToTake.Id} not available. ");
                    continue;
                }

                if (product.Quantity < productToTake.Quantity)
                {
                    errors.Append($"Product with id {productToTake.Id} doesn't have enough quantity. ");
                    continue;
                }

                product.Quantity -= productToTake.Quantity;
                products.Add(product);

                var takedProductDto = CreateTakedProductDto(product, productToTake.Quantity);
                takedProducts.Add(takedProductDto);
            }

            if (errors.Length > 0)
            {
                logger.LogError("Errors occurred: {Errors}", errors.ToString());
                throw new BadRequestException(errors.ToString());
            }

            await commandRepository.UpdateManyAsync(products, cancellationToken);
            await PublishEventsAsync(products, cancellationToken);

            logger.LogInformation("Successfully processed TakeProductsCommand with {Count} products", takedProducts.Count);

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
