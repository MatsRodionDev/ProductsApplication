using AutoMapper;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByUserId;
using ProductsService.Domain.Filters;

namespace ProductsService.Application.Common.Profiles
{
    public class UsersProductsFilterProfile : Profile
    {
        public UsersProductsFilterProfile()
        {
            CreateMap<GetProductsByUserIdQuery, GetUsersProductsFilters>();
        }
    }
}
