using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.UseCases.BasketUseCases.Create
{
    internal sealed class CreateBasketRequestHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateBasketRequestHandler> logger) : IRequestHandler<CreateBasketRequest>
    {
        public async Task Handle(CreateBasketRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Started handling CreateBasketRequest for UserId: {UserId}", request.UserId);

            var existingBasket = await unitOfWork.BasketRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (existingBasket is not null)
            {
                logger.LogWarning("Basket already exists for UserId: {UserId}", request.UserId);
                throw new BadRequestException("There is already a basket with this userId");
            }

            logger.LogInformation("No existing basket found for UserId: {UserId}, creating new basket.", request.UserId);

            await unitOfWork.BasketRepository.CreateAsync(
                new Basket(request.UserId),
                cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Basket successfully created for UserId: {UserId}", request.UserId);
        }
    }
}
