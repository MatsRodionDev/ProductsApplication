using AutoMapper;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByFilters;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByUserId;
using ProductsService.Domain.Filters;

namespace ProductsService.Application.Common.Profiles
{
    public class FiltersProfile : Profile
    {
        public FiltersProfile()
        {
            CreateMap<GetProductsByFiltersQuery, GetProductsFilters>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));

            CreateMap<GetProductsByUserIdQuery, GetUsersProductsFilters>();
        }
    }
}
