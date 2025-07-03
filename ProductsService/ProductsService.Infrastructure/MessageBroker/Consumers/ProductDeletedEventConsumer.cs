using MassTransit;
using Shared.Contracts;
using ProductsService.Domain.Interfaces;
using ProductsService.Application.Common.Interfaces.Services;
using ProductsService.Domain.Models;
using System.Threading;

namespace ProductsService.Infrastructure.MessageBroker.Consumers
{
    public sealed class ProductDeletedEventConsumer(
        IFileService fileService) : IConsumer<ProductDeletedEvent>
    {
        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {

            var removeImageTasks = context.Message.ImageNames
                .Select(async imageName =>
                {
                        await fileService.RemoveFileAsync(imageName);
                });

            await Task.WhenAll(removeImageTasks);
        }
    }
}
