using AutoMapper;
using OrderService.Application.Common.Models;
using ProductsService.API.Protos;

namespace OrderService.Infrastructure.Profiles
{
    public class TakedProductProfile : Profile
    {
        public TakedProductProfile()
        {
            CreateMap<ProductResponse, TakedProduct>();
        }
    }
}
