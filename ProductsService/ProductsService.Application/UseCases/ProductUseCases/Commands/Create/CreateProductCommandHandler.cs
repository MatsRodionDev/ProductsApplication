using AutoMapper;
using ProductsService.Application.Common.Abstractions;
using Shared.Contracts;
using ProductsService.Application.Common.Interfaces;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Create
{
    public class CreateProductCommandHandler(
        IFileService fileService,
        IProductCommandRepository productRepository,
        IMapper mapper,
        IEventBus eventBus) : ICommandHandler<CreateProductCommand>
    {
        public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = mapper.Map<Product>(request);

            if(request.ImageFiles is not null)
            {
                var imageTasks = request.ImageFiles
                    .Select(async file =>
                        {
                            var fileName = await fileService.UploadFileAsync(file, cancellationToken);

                            return new Image { ImageName = fileName };
                        });

                var images = await Task.WhenAll(imageTasks);
                product.Images = images.ToList();
            }

            await productRepository.AddAsync(product);
            await eventBus.PublishAsync(
                new ProductCreatedEvent(product.Id),
                cancellationToken);
        }
    }
}
