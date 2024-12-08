using MediatR;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Events.Product;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Exceptions;
using ProductsService.Domain.Interfaces;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Update
{
    public class UpdateProductCommandHandler
        (IProductCommandRepository repository,
        IMediator mediator): ICommandHandler<UpdateProductCommand>
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
            await mediator.Publish(new ProductUpdatedEvent(product.Id), cancellationToken);
        }
    }
}
