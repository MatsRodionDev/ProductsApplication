using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Contracts;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Update
{
    public class UpdateProductCommandHandler(
        IProductCommandRepository repository,
        IEventBus eventBus): ICommandHandler<UpdateProductCommand>
    {
        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product with such id doesnt exist");
            }

            if(request.UserId != product.UserId)
            {
                throw new UnauthorizedException("User with such id cannot change this product");
            }

            product.Update(
                request.Name,
                request.Description,
                request.Quantity,
                request.Price);

            await repository.UpdateAsync(product, cancellationToken);
            await eventBus.PublishAsync(
                new ProductUpdatedEvent(product.Id),
                cancellationToken);
        }
    }
}
