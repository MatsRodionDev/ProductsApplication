using AutoMapper;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Models;

namespace OrderService.Application.Common.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderResponseDto>();
        }
    }
}
