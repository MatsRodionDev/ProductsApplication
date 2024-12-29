using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Contracts;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.CreateCategory
{
    public class CreateCategoryToProductCommandHandler(
        IProductCommandRepository repository,
        IEventBus eventBus) : ICommandHandler<CreateCategoryToProductCommand>
    {
        public async Task Handle(CreateCategoryToProductCommand request, CancellationToken cancellationToken)
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

            var category = new Category
            {
                Name = request.Category
            };

            product.Categories.Add(category);

            await repository.UpdateAsync(product, cancellationToken);
            await eventBus.PublishAsync(
                new ProductUpdatedEvent(product.Id),
                cancellationToken);
        }
    }
}
