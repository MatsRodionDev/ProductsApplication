using AutoMapper;
using MediatR;
using ProductsService.Application.Common.Abstractions;
using ProductsService.Application.Common.Events.Product;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.Create
{
    public class CreateProductCommandHandler(
        IFileService fileService,
        IProductCommandRepository productRepository,
        IMapper mapper,
        IMediator mediator) : ICommandHandler<CreateProductCommand>
    {
        public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = mapper.Map<Product>(request);

            if(request.ImageFiles is not null)
            {
                foreach (var image in request.ImageFiles)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

                    product.Images.Add(new Image
                    {
                        ImageName = fileName
                    });

                    await fileService.UploadFileAsync(fileName, image, cancellationToken);
                }
            }

            await productRepository.AddAsync(product);
            await mediator.Publish(new ProductCreatedEvent(product.Id), cancellationToken);
        }
    }
}
