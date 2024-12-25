using MediatR;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.UseCases.BasketUseCases.DeleteItem
{
    internal sealed class DeleteItemFromBasketRequestHandler(
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteItemFromBasketRequest>
    {
        public async Task Handle(DeleteItemFromBasketRequest request, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.BasketRepository.GetByUserIdWithTrackingAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("There is no basket with this id");

            var item = basket.BasketItems
                .Find(i => i.ProductId == request.ProductId);

            if (item is not null)
            {
                basket.BasketItems.Remove(item);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
