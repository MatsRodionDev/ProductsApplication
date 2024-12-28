using AutoMapper;
using MediatR;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Contracts;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Create
{
    public class CreateProductCommandHandler(
        IFileService fileService,
        IProductCommandRepository productRepository,
        IMapper mapper,
        IPublisher publisher) : ICommandHandler<CreateProductCommand>
    {
        public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = mapper.Map<Product>(request);

            if(request.ImageFiles is not null)
            {
                var imageTasks = request.ImageFiles
                    .Select(async f =>
                        {
                            var fileName = await fileService.UploadFileAsync(f, cancellationToken);

                            return new Image { ImageName = fileName };
                        });

                var images = await Task.WhenAll(imageTasks);
                product.Images = images.ToList();
            }

            await productRepository.AddAsync(product);
            await publisher.Publish(
                new ProductCreatedEvent(product.Id),
                cancellationToken);
        }
    }
}
