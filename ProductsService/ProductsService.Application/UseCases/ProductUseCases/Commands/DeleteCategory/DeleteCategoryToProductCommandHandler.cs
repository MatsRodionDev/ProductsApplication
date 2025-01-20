using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteCategory
{
    public class DeleteCategoryToProductCommandHandler(
        IProductCommandRepository repository,
        IEventBus eventBus) : ICommandHandler<DeleteCategoryToProductCommand>
    {
        public async Task Handle(DeleteCategoryToProductCommand request, CancellationToken cancellationToken)
        {
            var product = await repository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product with such id doesnt exist");
            }

            if (product.UserId != request.UserId)
            {
                throw new UnauthorizedException("User with such id cannot change this product");
            }

            var category = product.Categories
                .Find(i => i.Id == request.CategoryId);

            if (category is null)
            {
                throw new NotFoundException("Category with such id doesnt exist");
            }

            product.Categories.Remove(category);

            await repository.UpdateAsync(product, cancellationToken);
            await eventBus.PublishAsync(
                new ProductUpdatedEvent(product.Id, product.Quantity), 
                cancellationToken);
        }
    }
}
