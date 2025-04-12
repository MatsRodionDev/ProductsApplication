using AutoMapper;
using OrderService.Application.Common.Dtos;
using ProductsService.API.Protos;

namespace OrderService.Infrastructure.Profiles
{ 
    public class ProductRequestProfile : Profile
    {
        public ProductRequestProfile()
        {
            CreateMap<TakeProductDto, ProductRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId.ToString()));
        }
    }
}
