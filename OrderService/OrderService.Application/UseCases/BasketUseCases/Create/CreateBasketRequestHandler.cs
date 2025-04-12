using MediatR;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.BasketUseCases.Create
{
    internal sealed class CreateBasketRequestHandler(
        IUnitOfWork unitOfWork) : IRequestHandler<CreateBasketRequest>
    {

        public async Task Handle(CreateBasketRequest request, CancellationToken cancellationToken)
        {
            var existingBasket = await unitOfWork.BasketRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (existingBasket is not null)
            {
                throw new BadRequestException("There is already a basket with this userId");
            }

            await unitOfWork.BasketRepository.CreateAsync(
                new Basket(request.UserId),
                cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
