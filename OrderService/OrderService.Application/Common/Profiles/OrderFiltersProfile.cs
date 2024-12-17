using AutoMapper;
using OrderService.Application.UseCases.OrderUseCases.GetByFilters;
using OrderService.Domain.Filters;

namespace OrderService.Application.Common.Profiles
{
    internal class OrderFiltersProfile : Profile
    {
        public OrderFiltersProfile()
        {
            CreateMap<GetOrdersByFiltersRequest, OrderFilters>();
        }
    }
}
