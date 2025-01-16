using AutoMapper;
using OrderService.Application.Common.Models;
using ProductsService.API.Protos;

namespace OrderService.Infrastructure.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductByIdResponse, Product>();    
        }
    }
}
