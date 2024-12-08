using MediatR;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Events.Product;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteCategory
{
    public class DeleteCategoryToProductCommandHandler(
        IProductCommandRepository repository,
        IMediator mediator) : ICommandHandler<DeleteCategoryToProductCommand>
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
            await mediator.Publish(
                new ProductUpdatedEvent(product.Id), 
                cancellationToken);
        }
    }
}
