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
        IUnitOfWork unitOfWork,
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

            try
            {
                await unitOfWork.Products.AddAsync(product, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await eventBus.PublishAsync(
                    new ProductCreatedEvent(product.Id),
                    cancellationToken);
            }
            catch
            {
                var removeImageTasks = product.Images
                    .Select(async image =>
                    {
                        await fileService.RemoveFileAsync(image.ImageName, cancellationToken);
                    });

                await Task.WhenAll(removeImageTasks);

                throw;
            }
        }
    }
}
