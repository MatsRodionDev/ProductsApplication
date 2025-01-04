using AutoMapper;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Domain.Models;

namespace ProductsService.Application.Common.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryResponseDto>()
                .ReverseMap();
        }
    }
}
