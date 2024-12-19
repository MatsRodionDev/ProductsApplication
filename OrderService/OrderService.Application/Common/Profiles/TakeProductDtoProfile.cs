using AutoMapper;
using OrderService.Application.Common.Dtos;

namespace OrderService.Application.Common.Profiles
{
    public class TakeProductDtoProfile : Profile
    {
        public TakeProductDtoProfile()
        {
            CreateMap<TakeProductDto, ReturnProductDto>();
        }
    }
}
