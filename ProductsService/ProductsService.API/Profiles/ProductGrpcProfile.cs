using AutoMapper;
using ProductsService.API.Protos;
using ProductsService.Application.Common.Dto.Requests;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Application.UseCases.ProductUseCases.Commands.TakeOne;

namespace ProductsService.API.Profiles
{
    public class ProductGrpcProfile : Profile
    {
        public ProductGrpcProfile() 
        { 
            CreateMap<ProductResponseDto, ProductByIdResponse>();
            CreateMap<ProductRequest, TakeProductRequestDto>();
            CreateMap<ProductRequest, TakeOneProductCommand>();
            CreateMap<TakedProductResponseDto, ProductResponse>();
        }
    }
}
