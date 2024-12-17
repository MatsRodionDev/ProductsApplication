using AutoMapper;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Models;

namespace OrderService.Application.Common.Profiles
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemResponseDto>();
        }
    }
}
