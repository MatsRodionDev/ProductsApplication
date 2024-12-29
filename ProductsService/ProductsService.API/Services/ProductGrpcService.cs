using AutoMapper;
using Grpc.Core;
using MediatR;
using ProductsService.API.Protos;
using ProductsService.Application.Common.Dto.Requests;
using ProductsService.Application.UseCases.ProductUseCases.Commands.Take;
using ProductsService.Application.UseCases.ProductUseCases.Commands.TakeOne;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetById;

namespace ProductsService.API.Services
{
    public class ProductGrpcService(
        IMediator mediator,
        IMapper mapper) : Products.ProductsBase
    {
        public override async Task<ProductByIdResponse> GetProduct(ProductByIdRequest request, ServerCallContext context)
        {
            var product = await mediator.Send(
                new GetProductByIdQuery(Guid.Parse(request.Id)), 
                context.CancellationToken);

            return mapper.Map<ProductByIdResponse>(product);
        }

        public override async Task<ProductsListResponse> TakeProducts(ProductsListRequest request, ServerCallContext context)
        {
            var productsToTake = mapper.Map<List<TakeProductRequestDto>>(request.Products);

            var command = new TakeProductsCommand(productsToTake);
            var products = await mediator.Send(command, context.CancellationToken);

            var productsResponse = mapper.Map<List<ProductResponse>>(products);

            var response = new ProductsListResponse();
            response.Products.AddRange(productsResponse);

            return response;
        }

        public override async Task<ProductResponse> TakeOneProduct(ProductRequest request, ServerCallContext context)
        {
            var command = mapper.Map<TakeOneProductCommand>(request);
            var product = await mediator.Send(command, context.CancellationToken);

            return mapper.Map<ProductResponse>(product);
        }
    }
}
