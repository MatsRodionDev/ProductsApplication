using AutoMapper;
using OrderService.Application.Common.Dtos;
using OrderService.Application.UseCases.OrderUseCases.Create;
using OrderService.Domain.Models;

namespace OrderService.Application.Common.Profiles
{
    public class TakeProductDtoProfile : Profile
    {
        public TakeProductDtoProfile()
        {
            CreateMap<CreateOrderRequest, TakeProductDto> ();
            CreateMap<BasketItem, TakeProductDto>();
        }
    }
}
