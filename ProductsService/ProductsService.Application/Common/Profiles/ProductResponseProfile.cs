using AutoMapper;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Domain.Models;

namespace ProductsService.Application.Common.Profiles
{
    public class ProductResponseProfile : Profile
    {
        public ProductResponseProfile()
        {
            CreateMap<Product, ProductResponseDto>()
                .ReverseMap();
        }
    }
}
