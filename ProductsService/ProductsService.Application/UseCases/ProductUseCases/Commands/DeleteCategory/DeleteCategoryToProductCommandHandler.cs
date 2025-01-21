using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteCategory
{
    public class DeleteCategoryToProductCommandHandler(
        IProductCommandRepository repository,
        IEventBus eventBus,
        ILogger<DeleteCategoryToProductCommandHandler> logger) : ICommandHandler<DeleteCategoryToProductCommand>
    {
        public async Task Handle(DeleteCategoryToProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting to handle DeleteCategoryToProductCommand for ProductId: {ProductId}, CategoryId: {CategoryId}", request.ProductId, request.CategoryId);

            var product = await repository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                logger.LogError("Product with id {ProductId} not found", request.ProductId);
                throw new NotFoundException("Product with such id doesn't exist");
            }

            if (product.UserId != request.UserId)
            {
                logger.LogError("User with id {UserId} is not authorized to delete Category from Product with id {ProductId}", request.UserId, request.ProductId);
                throw new UnauthorizedException("User with such id cannot change this product");
            }

            var category = product.Categories
                .Find(i => i.Id == request.CategoryId);

            if (category is null)
            {
                logger.LogError("Category with id {CategoryId} not found in Product with id {ProductId}", request.CategoryId, request.ProductId);
                throw new NotFoundException("Category with such id doesn't exist");
            }

            product.Categories.Remove(category);
            await repository.UpdateAsync(product, cancellationToken);

            logger.LogInformation("Product with id {ProductId} successfully updated after category removal", request.ProductId);

            await eventBus.PublishAsync(
                new ProductUpdatedEvent(product.Id, product.Quantity),
                cancellationToken);

            logger.LogInformation("Event ProductUpdatedEvent for ProductId: {ProductId} published successfully", request.ProductId);
        }
    }
}
