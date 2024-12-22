using AutoMapper;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Domain.Models;

namespace ProductsService.Application.Common.Profiles
{
    public class FiltersProfile : Profile
    {
        public FiltersProfile()
        {
            CreateMap<Product, ProductResponseDto>()
                .ReverseMap();
        }
    }
}
