using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.CreateCategory
{
    public class CreateCategoryToProductCommandHandler(
        IUnitOfWork unitOfWork) : ICommandHandler<CreateCategoryToProductCommand>
    {
        public async Task Handle(CreateCategoryToProductCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product with such id doesnt exist");
            }

            if (product.UserId != request.UserId)
            {
                throw new UnauthorizedException("User with such id cannot change this product");
            }

            var category = await unitOfWork.Categories.GetByIdAsync(request.CategoryId, cancellationToken);

            if (category is null)
            {
                throw new NotFoundException("Category with such id doesnt exist");
            }

            product.Categories.Add(category);

            unitOfWork.Products.Update(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
