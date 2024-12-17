using MediatR;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.BasketUseCases.Create
{
    internal sealed class CreateBasketRequestHandler(
        IUnitOfWork unitOfWork,
        IBasketRepository basketRepository) : IRequestHandler<CreateBasketRequest>
    {

        public async Task Handle(CreateBasketRequest request, CancellationToken cancellationToken)
        {
            var existingBasket = await basketRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (existingBasket is not null)
            {
                throw new BadRequestException("There is already a basket with this userId");
            }

            await basketRepository.CreateAsync(
                new Basket(request.UserId),
                cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
