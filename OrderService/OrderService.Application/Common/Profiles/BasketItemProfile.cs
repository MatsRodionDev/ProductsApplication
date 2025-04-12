using AutoMapper;
using OrderService.Application.Common.Dtos;
using OrderService.Domain.Models;

namespace OrderService.Application.Common.Profiles
{
    public class BasketItemProfile : Profile
    {
        public BasketItemProfile()
        {
            CreateMap<BasketItem, TakeProductDto>();
        }
    }
}
