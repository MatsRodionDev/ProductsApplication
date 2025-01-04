using AutoMapper;
using OrderService.Application.Common.Dtos;
using OrderService.Application.UseCases.OrderUseCases.Create;
using OrderService.Domain.Models;

namespace OrderService.Application.Common.Profiles
{
    public class CreateOrderProfile : Profile
    {
        public CreateOrderProfile()
        {
            CreateMap<CreateOrderRequest, TakeProductDto> ();
        }
    }
}
