using AutoMapper;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByFilters;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByUserId;
using ProductsService.Domain.Filters;

namespace ProductsService.Application.Common.Profiles
{
    public class ProductsFiltersProfile : Profile
    {
        public ProductsFiltersProfile()
        {
            CreateMap<GetProductsByFiltersQuery, GetProductsFilters>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));
        }
    }
}
